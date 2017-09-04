// -----------------------------------------------------------------------
//  <copyright file="ValidateCodeType.cs" company="MO开源团队">
//      Copyright (c) 2014 MO. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-18 16:53</last-date>
// -----------------------------------------------------------------------

namespace MO.Utility.Drawing
{
    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum ValidateCodeType
    {
        /// <summary>
        /// 纯数值
        /// </summary>
        Number,

        /// <summary>
        /// 数值与字母的组合
        /// </summary>
        NumberAndLetter,

        /// <summary>
        /// 汉字
        /// </summary>
        Hanzi
    }
}