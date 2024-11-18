﻿namespace Noc_App.Helpers
{
    public static class DeviceIdHelper
    {
        public static string GenerateDeviceId(HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userAgent));
        }
    }

}