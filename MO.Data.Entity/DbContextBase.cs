using MO.Core.Configs;
using MO.Core.Data;
using MO.Core.Logging;
using MO.Data.Entity.Properties;
using MO.Utility.Exceptions;
using MO.Utility.Extensions;
using MO.Utility.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TransactionalBehavior = MO.Core.Data.TransactionalBehavior;

namespace MO.Data.Entity
{
    /// <summary>
    /// 数据库上下文基类
    /// </summary>
    public abstract class DbContextBase<TDbContext> : DbContext, IUnitOfWork
        where TDbContext : DbContext, IUnitOfWork, new()
    {
        protected readonly ILogger Logger = LogManager.GetLogger(typeof(TDbContext));
        private static DbContextConfig _contextConfig;

        #region 构造函数

        /// <summary>
        /// 初始化一个<see cref="DbContextBase{TDbContext}"/>类型的新实例
        /// </summary>
        protected DbContextBase()
            : base(GetConnectionStringName())
        {
            //监听数据库sql 语句
#if DEBUG
            DbInterception.Add(new EFIntercepterLogging());
#endif
        }

        /// <summary>
        /// 初始化一个<see cref="DbContextBase{TDbContext}"/>类型的新实例
        /// </summary>
        protected DbContextBase(DbCompiledModel model)
            : base(model)
        { }

        /// <summary>
        /// 初始化一个<see cref="DbContextBase{TDbContext}"/>类型的新实例
        /// </summary>
        protected DbContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        /// <summary>
        /// 初始化一个<see cref="DbContextBase{TDbContext}"/>类型的新实例
        /// </summary>
        protected DbContextBase(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        { }

        /// <summary>
        /// 初始化一个<see cref="DbContextBase{TDbContext}"/>类型的新实例
        /// </summary>
        protected DbContextBase(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        { }

        /// <summary>
        /// 初始化一个<see cref="DbContextBase{TDbContext}"/>类型的新实例
        /// </summary>
        protected DbContextBase(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        { }

        /// <summary>
        /// 初始化一个<see cref="DbContextBase{TDbContext}"/>类型的新实例
        /// </summary>
        protected DbContextBase(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        { }

        #endregion

        /// <summary>
        /// 获取数据库连接字符串的名称
        /// </summary>
        /// <returns>数据库连接字符串对应的名称</returns>
        private static string GetConnectionStringName()
        {
            DbContextConfig contextConfig = GetDbContextConfig();
            if (contextConfig == null || !contextConfig.Enabled)
            {
                return typeof(TDbContext).ToString();
            }
            string name = contextConfig.ConnectionStringName;
            if (ConfigurationManager.ConnectionStrings[name] == null)
            {
                throw new InvalidOperationException(Resources.DbContextBase_ConnectionStringNameNotExist.FormatWith(name));
            }
            return name;
        }

        /// <summary>
        /// 获取OA框架数据上下文配置
        /// </summary>
        /// <returns></returns>
        private static DbContextConfig GetDbContextConfig()
        {
            return _contextConfig ?? (_contextConfig = MOConfig.Instance.DataConfig.ContextConfigs
                .FirstOrDefault(m => m.ContextType == typeof(TDbContext)));
        }

        /// <summary>
        /// 获取或设置 服务提供者
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 获取或设置 数据日志缓存
        /// </summary>
        public IDataLogCache DataLogCache { get; set; }

        /// <summary>
        /// 获取 是否允许数据库日志记录
        /// </summary>
        protected virtual bool DataLoggingEnabled
        {
            get
            {
                DbContextConfig contextConfig = GetDbContextConfig();
                if (contextConfig == null || !contextConfig.Enabled)
                {
                    return false;
                }
                return contextConfig.DataLoggingEnabled;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //移除一对多的级联删除
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //注册实体配置信息
            IEntityMapper[] entityMappers = DbContextManager.Instance.GetEntityMappers(typeof(TDbContext)).ToArray();
            foreach (IEntityMapper mapper in entityMappers)
            {
                mapper.RegistTo(modelBuilder.Configurations);
            }
        }

        #region Implementation of IUnitOfWork

        /// <summary>
        /// 获取或设置 是否开启事务提交
        /// </summary>
        public bool TransactionEnabled { get; set; }

        /// <summary>
        /// 对数据库执行给定的 DDL/DML 命令。 
        /// 与接受 SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。 您可以在 SQL 查询字符串中包含参数占位符，然后将参数值作为附加参数提供。 
        /// 您提供的任何参数值都将自动转换为 DbParameter。 unitOfWork.ExecuteSqlCommand("UPDATE dbo.Posts SET Rating = 5 WHERE Author = @p0", userSuppliedAuthor); 
        /// 或者，您还可以构造一个 DbParameter 并将它提供给 SqlQuery。 这允许您在 SQL 查询字符串中使用命名参数。 unitOfWork.ExecuteSqlCommand("UPDATE dbo.Posts SET Rating = 5 WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));
        /// </summary>
        /// <param name="transactionalBehavior">对于此命令控制事务的创建。</param>
        /// <param name="sql">命令字符串。</param>
        /// <param name="parameters">要应用于命令字符串的参数。</param>
        /// <returns>执行命令后由数据库返回的结果。</returns>
        public virtual int ExecuteSqlCommand(TransactionalBehavior transactionalBehavior, string sql, params object[] parameters)
        {
            System.Data.Entity.TransactionalBehavior behavior = transactionalBehavior == TransactionalBehavior.DoNotEnsureTransaction
                ? System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction
                : System.Data.Entity.TransactionalBehavior.EnsureTransaction;
            return Database.ExecuteSqlCommand(behavior, sql, parameters);
        }

        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。 类型可以是包含与从查询返回的列名匹配的属性的任何类型，也可以是简单的基元类型。 该类型不必是实体类型。
        ///  即使返回对象的类型是实体类型，上下文也决不会跟踪此查询的结果。 使用 SqlQuery(String, Object[]) 方法可返回上下文跟踪的实体。 
        /// 与接受 SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。 您可以在 SQL 查询字符串中包含参数占位符，然后将参数值作为附加参数提供。 
        /// 您提供的任何参数值都将自动转换为 DbParameter。 unitOfWork.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor); 
        /// 或者，您还可以构造一个 DbParameter 并将它提供给 SqlQuery。 这允许您在 SQL 查询字符串中使用命名参数。 unitOfWork.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));
        /// </summary>
        /// <typeparam name="TElement">查询所返回对象的类型。</typeparam>
        /// <param name="sql">SQL 查询字符串。</param>
        /// <param name="parameters">要应用于 SQL 查询字符串的参数。 如果使用输出参数，则它们的值在完全读取结果之前不可用。 这是由于 DbDataReader 的基础行为而导致的，有关详细信息，请参见 http://go.microsoft.com/fwlink/?LinkID=398589。</param>
        /// <returns></returns>
        public virtual IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return Database.SqlQuery<TElement>(sql, parameters);
        }

        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回给定类型的元素。 类型可以是包含与从查询返回的列名匹配的属性的任何类型，也可以是简单的基元类型。 该类型不必是实体类型。 即使返回对象的类型是实体类型，上下文也决不会跟踪此查询的结果。 使用 SqlQuery(String, Object[]) 方法可返回上下文跟踪的实体。 与接受 SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。 您可以在 SQL 查询字符串中包含参数占位符，然后将参数值作为附加参数提供。 您提供的任何参数值都将自动转换为 DbParameter。 context.Database.SqlQuery(typeof(Post), "SELECT * FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor); 或者，您还可以构造一个 DbParameter 并将它提供给 SqlQuery。 这允许您在 SQL 查询字符串中使用命名参数。 context.Database.SqlQuery(typeof(Post), "SELECT * FROM dbo.Posts WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));
        /// </summary>
        /// <param name="elementType">查询所返回对象的类型。</param>
        /// <param name="sql">SQL 查询字符串。</param>
        /// <param name="parameters">要应用于 SQL 查询字符串的参数。 如果使用输出参数，则它们的值在完全读取结果之前不可用。 这是由于 DbDataReader 的基础行为而导致的，有关详细信息，请参见 http://go.microsoft.com/fwlink/?LinkID=398589。</param>
        /// <returns></returns>
        public virtual IEnumerable SqlQuery(Type elementType, string sql, params object[] parameters)
        {
            return Database.SqlQuery(elementType, sql, parameters);
        }

        #endregion

        /// <summary>
        /// 提交当前单元操作的更改
        /// </summary>
        /// <returns>操作影响的行数</returns>
        public override int SaveChanges()
        {
            return SaveChanges(true);
        }

        /// <summary>
        /// 提交当前单元操作的更改
        /// </summary>
        /// <param name="validateOnSaveEnabled">提交保存时是否验证实体约束有效性。</param>
        /// <returns>操作影响的行数</returns>
        internal virtual int SaveChanges(bool validateOnSaveEnabled)
        {
            bool isReturn = Configuration.ValidateOnSaveEnabled != validateOnSaveEnabled;
            try
            {
                Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
                //记录实体操作日志
                List<DataLog> logs = new List<DataLog>();
                if (DataLoggingEnabled)
                {
                    logs = this.GetEntityDataLogs(ServiceProvider).ToList();
                }
                int count = 0;
                try
                {
                    count = base.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    IEnumerable<DbEntityValidationResult> errorResults = ex.EntityValidationErrors;
                    List<string> ls = (from result in errorResults
                                       let lines = result.ValidationErrors.Select(error => "{0}: {1}".FormatWith(error.PropertyName, error.ErrorMessage)).ToArray()
                                       select "{0}({1})".FormatWith(result.Entry.Entity.GetType().FullName, lines.ExpandAndToString(", "))).ToList();
                    string message = "数据验证引发异常——" + ls.ExpandAndToString(" | ");
                    throw new DataException(message, ex);
                }
                if (count > 0 && DataLoggingEnabled)
                {
                    foreach (DataLog log in logs)
                    {
                        DataLogCache.AddDataLog(log);
                    }
                    //Logger.Info(logs, true);
                }
                TransactionEnabled = false;
                return count;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException != null && e.InnerException.InnerException is SqlException)
                {
                    SqlException sqlEx = e.InnerException.InnerException as SqlException;
                    string msg = DataHelper.GetSqlExceptionMessage(sqlEx.Number);
                    throw new MOException("提交数据更新时发生异常：" + msg, sqlEx);
                }
                throw;
            }
            finally
            {
                if (isReturn)
                {
                    Configuration.ValidateOnSaveEnabled = !validateOnSaveEnabled;
                }
            }
        }

        /// <summary>
        /// 对数据库执行给定的 DDL/DML 命令。 
        /// 与接受 SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。 您可以在 SQL 查询字符串中包含参数占位符，然后将参数值作为附加参数提供。 
        /// 您提供的任何参数值都将自动转换为 DbParameter。 unitOfWork.ExecuteSqlCommand("UPDATE dbo.Posts SET Rating = 5 WHERE Author = @p0", userSuppliedAuthor); 
        /// 或者，您还可以构造一个 DbParameter 并将它提供给 SqlQuery。 这允许您在 SQL 查询字符串中使用命名参数。 unitOfWork.ExecuteSqlCommand("UPDATE dbo.Posts SET Rating = 5 WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));
        /// </summary>
        /// <param name="transactionalBehavior">对于此命令控制事务的创建。</param>
        /// <param name="sql">命令字符串。</param>
        /// <param name="parameters">要应用于命令字符串的参数。</param>
        /// <returns>执行命令后由数据库返回的结果。</returns>
        public virtual async Task<int> ExecuteSqlCommandAsync(TransactionalBehavior transactionalBehavior, string sql, params object[] parameters)
        {
            System.Data.Entity.TransactionalBehavior behavior = transactionalBehavior == TransactionalBehavior.DoNotEnsureTransaction
                ? System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction
                : System.Data.Entity.TransactionalBehavior.EnsureTransaction;
            return await Database.ExecuteSqlCommandAsync(behavior, sql, parameters);
        }

        /// <summary>
        /// 异步提交当前单元操作的更改。
        /// </summary>
        /// <returns>操作影响的行数</returns>
        public override Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(true);
        }

        /// <summary>
        /// 提交当前单元操作的更改。
        /// </summary>
        /// <param name="validateOnSaveEnabled">提交保存时是否验证实体约束有效性。</param>
        /// <returns>操作影响的行数</returns>
        internal virtual async Task<int> SaveChangesAsync(bool validateOnSaveEnabled)
        {
            bool isReturn = Configuration.ValidateOnSaveEnabled != validateOnSaveEnabled;
            try
            {
                Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
                //记录实体操作日志
                List<DataLog> logs = new List<DataLog>();
                if (DataLoggingEnabled)
                {
                    logs = (await this.GetEntityOperateLogsAsync(ServiceProvider)).ToList();
                }
                int count = 0;
                try
                {
                    count = await base.SaveChangesAsync();
                }
                catch (DbEntityValidationException ex)
                {
                    IEnumerable<DbEntityValidationResult> errorResults = ex.EntityValidationErrors;
                    List<string> ls = (from result in errorResults
                                       let lines = result.ValidationErrors.Select(error => "{0}: {1}".FormatWith(error.PropertyName, error.ErrorMessage)).ToArray()
                                       select "{0}({1})".FormatWith(result.Entry.Entity.GetType().FullName, lines.ExpandAndToString(", "))).ToList();
                    string message = "数据验证引发异常——" + ls.ExpandAndToString(" | ");
                    throw new DataException(message, ex);
                }
                if (count > 0 && DataLoggingEnabled)
                {
                    foreach (DataLog log in logs)
                    {
                        DataLogCache.AddDataLog(log);
                    }
                    //Logger.Info(logs, true);
                }
                TransactionEnabled = false;
                return count;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException != null && e.InnerException.InnerException is SqlException)
                {
                    SqlException sqlEx = e.InnerException.InnerException as SqlException;
                    string msg = DataHelper.GetSqlExceptionMessage(sqlEx.Number);
                    throw new MOException("提交数据更新时发生异常：" + msg, sqlEx);
                }
                throw;
            }
            finally
            {
                if (isReturn)
                {
                    Configuration.ValidateOnSaveEnabled = !validateOnSaveEnabled;
                }
            }
        }
    }



    /// <summary>
    /// 监听数据库sql 语句
    /// </summary>
    public class EFIntercepterLogging : DbCommandInterceptor
    {

        private readonly Stopwatch _stopwatch = new Stopwatch();

        public override void ScalarExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {

            base.ScalarExecuting(command, interceptionContext);

            _stopwatch.Restart();

        }

        public override void ScalarExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {

            _stopwatch.Stop();

            if (interceptionContext.Exception != null)
            {

                Trace.TraceError("Exception:{1} \r\n --> Error executing command: {0}", command.CommandText, interceptionContext.Exception.ToString());

            }

            else
            {

                Trace.TraceInformation("\r\n执行时间:{0} 毫秒\r\n-->ScalarExecuted.Command:{1}\r\n", _stopwatch.ElapsedMilliseconds, command.CommandText);

            }

            base.ScalarExecuted(command, interceptionContext);

        }

        public override void NonQueryExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {

            base.NonQueryExecuting(command, interceptionContext);

            _stopwatch.Restart();

        }

        public override void NonQueryExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {

            _stopwatch.Stop();

            if (interceptionContext.Exception != null)
            {
                Trace.TraceError("Exception:{1} \r\n --> Error executing command:\r\n {0}", command.CommandText, interceptionContext.Exception.ToString());

            }

            else
            {

                Trace.TraceInformation("\r\n执行时间:{0} 毫秒\r\n-->NonQueryExecuted.Command:\r\n{1}", _stopwatch.ElapsedMilliseconds, command.CommandText);

            }

            //Log.SendTrace(Getstr(command));
            base.NonQueryExecuted(command, interceptionContext);

        }

        public override void ReaderExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
        {

            base.ReaderExecuting(command, interceptionContext);

            _stopwatch.Restart();

        }

        public override void ReaderExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
        {

            _stopwatch.Stop();

            if (interceptionContext.Exception != null)
            {

                Trace.TraceError("Exception:{1} \r\n --> Error executing command:\r\n {0}", command.CommandText, interceptionContext.Exception.ToString());

            }

            else
            {

                Trace.TraceInformation("\r\n执行时间:{0} 毫秒 \r\n -->ReaderExecuted.Command:\r\n{1}", _stopwatch.ElapsedMilliseconds, command.CommandText);

            }

            base.ReaderExecuted(command, interceptionContext);
        }

        public string Getstr(System.Data.Common.DbCommand command)
        {
            string aa = command.CommandText + "\r\n";
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                aa += string.Format("【{0}:{1}】，", command.Parameters[i].ParameterName, command.Parameters[i].Value);
            }
            return aa;
        }
    }
}
