using System.Collections.Generic;
using Core.Common.Enums;
using Core.Common.Models;

namespace Core.Common.Services
{
    public sealed class ResponseMessageCommonService
    {
        public ResponseMessage Success { get; }

        public ResponseMessage Unknown { get; }
        
        public ResponseMessage LoginFail { get; }
        
        public ResponseMessage RefreshFail { get; }
        
        public ResponseMessage UserNameExist { get; }

        public ResponseMessageCommonService()
        {
            Success = new ResponseMessage
            {
                Code = 1,
                Success = true,
                Message = new Dictionary<EnumLanguage, string>
                {
                    {EnumLanguage.Tr, "İşlem Başarılı!"},
                    {EnumLanguage.En, "Completed Successfully!"}
                }
            };

            Unknown = new ResponseMessage
            {
                Code = 2,
                Success = false,
                Message = new Dictionary<EnumLanguage, string>
                {
                    {EnumLanguage.Tr, "Bilinmeyen bir hata ile karşılaşıldı!"},
                    {EnumLanguage.En, "Unknown error occurred!"}
                }
            };

            LoginFail = new ResponseMessage
            {
                Code = 3,
                Success = false,
                Message = new Dictionary<EnumLanguage, string>
                {
                    {EnumLanguage.Tr, "Kullanıcı adı veya şifre hatalı! Lütfen tekrar deneyiniz."},
                    {EnumLanguage.En, "Username or password incorrect! Please try again."}
                }
            };

            RefreshFail = new ResponseMessage
            {
                Code = 4,
                Success = false,
                Message = new Dictionary<EnumLanguage, string>
                {
                    {EnumLanguage.Tr, "Anahtar yenileme işlemi başarısız!"},
                    {EnumLanguage.En, "Token refresh process failed!"}
                }
            };

            UserNameExist = new ResponseMessage
            {
                Code = 5,
                Success = false,
                Message = new Dictionary<EnumLanguage, string>
                {
                    {EnumLanguage.Tr, "Bu kullanıcı adı zaten alınmış!"},
                    {EnumLanguage.En, "This username already taken!"}
                }
            };
        }
    }
}