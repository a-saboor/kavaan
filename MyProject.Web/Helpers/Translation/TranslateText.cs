using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.Helpers.Translation
{
	public static class Translation
	{
		public static string Translate(string text, bool AllowSplit = false)
		{
			try
			{
				if (AllowSplit)
				{
					string value = string.Empty;
					var split = text.Split(' ');
					string ar = ArabicValues[split.LastOrDefault()];

					for (int i = 0; i < split.Count() - 1; i++)
						value += split[i] + " ";

					return value + ar;
				}
				else
				{
					return ArabicValues[text];
				}
			}
			catch (Exception ex)
			{
				return text;
			}
		}

		static Dictionary<string, string> ArabicValues = new Dictionary<string, string>
		{
			{"Login Successful!","تسجيل الدخول بنجاح!"}
			,{  "Wrong Password!", "كلمة مرور خاطئة!"}
			,{  "OTP verification needed, we just need to verify your contact before you can access!", "مطلوب التحقق من OTP، نحتاج فقط للتحقق من جهة الاتصال الخاصة بك قبل أن تتمكن من الوصول إليها!"}
			,{  "Email verification needed, we just need to verify your email address before you can access!", "مطلوب التحقق من البريد الإلكتروني، نحتاج فقط للتحقق من عنوان بريدك الإلكتروني قبل أن تتمكن من الوصول!"}
			,{  "Account suspended!", "حساب معلق!"}
			,{  "Invalid Contact or Password!", "جهة اتصال غير صالحة أو كلمة مرور!"}
		};
	}
}