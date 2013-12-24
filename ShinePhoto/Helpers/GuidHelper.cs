using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ShinePhoto.Helpers
{
    /// <summary>
    /// 生成 GUID 序列串帮助类 
    /// </summary>
    public class GuidHelper
    {
        public static string Generate(GUIDFormat format, bool upperCase)
        {
            string result = string.Empty;

            switch (format)
            {
                case GUIDFormat.N:
                    result = Guid.NewGuid().ToString("N");
                    break;
                case GUIDFormat.D:
                    result = Guid.NewGuid().ToString("D");
                    break;
                case GUIDFormat.B:
                    result = Guid.NewGuid().ToString("B");
                    break;
                case GUIDFormat.P:
                    result = Guid.NewGuid().ToString("P");
                    break;
                case GUIDFormat.X:
                    result = Guid.NewGuid().ToString("X");
                    break;
                default:
                    result = Guid.NewGuid().ToString("N");
                    break;
            }

            if (upperCase)
            {
                result = result.ToUpper();
            }

            return result;
        }
    }

    public enum GUIDFormat
    { 
        [Description("32 位：00000000000000000000000000000000")]
        N,
        [Description("由连字符分隔的 32 位数字：00000000-0000-0000-0000-000000000000")]
        D,
        [Description("括在大括号中、由连字符分隔的 32 位数字：{00000000-0000-0000-0000-000000000000}")]
        B,
        [Description("括在圆括号中、由连字符分隔的 32 位数字：(00000000-0000-0000-0000-000000000000)")]
        P,
        [Description("括在大括号的 4 个十六进制值，其中第 4 个值是 8 个十六进制值的子集（也括在大括号中）：{0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}")]
        X
    }
}
