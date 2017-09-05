﻿using MO.Core.Configs.ConfigFile;
using MO.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Core.Configs
{
    /// <summary>
    /// 日志记录入口配置
    /// </summary>
    public class LoggingEntryConfig
    {
        /// <summary>
        /// 初始化一个<see cref="LoggingEntryConfig"/>类型的新实例
        /// </summary>
        public LoggingEntryConfig()
        {
            Enabled = true;
            EntryLogLevel = LogLevel.All;
        }

        /// <summary>
        /// 初始化一个<see cref="LoggingEntryConfig"/>类型的新实例
        /// </summary>
        internal LoggingEntryConfig(LoggingEntryElement element)
        {
            Enabled = element.Enabled;
            EntryLogLevel = element.EntryLogLevel;
        }


        /// <summary>
        /// 获取或设置 从入口控制是否允许记录日志
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 获取或设置 入口允许记录的日志等级
        /// </summary>
        public LogLevel EntryLogLevel { get; set; }
    }
}
