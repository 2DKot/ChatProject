using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    delegate string reactionOfServer();
    delegate string transformationText(string restParameters);
    static class Actions
    {
        static public Dictionary<int, reactionOfServer> serviceCodeToDefinition = new Dictionary<int, reactionOfServer>();
        static public Dictionary<string, transformationText> commandToHandler = new Dictionary<string, transformationText>();
        static Actions()
        {
            serviceCodeToDefinition.Add(1, ErrorIncorrectFormatOfMessage);
            serviceCodeToDefinition.Add(2, UnknownErrorOfSendingMessage);
            serviceCodeToDefinition.Add(3, ErrorUnexistingUser);
            serviceCodeToDefinition.Add(50, NickHasChangedSuccesful);
            serviceCodeToDefinition.Add(51, ErrorTheNickIsUsed);
            serviceCodeToDefinition.Add(52, ErrorTheNickIsIncorrect);
            //*****************************//
            commandToHandler.Add("MSG", MSG);
            commandToHandler.Add("ERROR", ERROR);
            commandToHandler.Add("PRIVMSG)", PRIVMSG);
        }
        static private string ErrorIncorrectFormatOfMessage()
        {
            return "Некорретный формат сообщения. Формат по протоколу: cmnd param.";
        }
        static private string UnknownErrorOfSendingMessage()
        {
            return "Неизвестная ошибка при передаче сообщения.";
        }
        static private string ErrorUnexistingUser()
        {
            return "Личное сообщение не было передано из-за отсутствия пользователя.";
        }
        static private string NickHasChangedSuccesful()
        {
            return "Ник-нейм пользователя был удачно сменен.";
        }
        static private string ErrorTheNickIsUsed()
        {
            return "Данный ник-нейм занят.";
        }
        static private string ErrorTheNickIsIncorrect()
        {
            return "Данный ник-нейм не соответствует правилам сервера";
        }

        //*****************************//

        static private string MSG(string restParameters)
        {
            return ("Всем от " + restParameters);
        }
        static private string PRIVMSG(string restParameters)
        {
            return ("ЫЫЫ");
        }
        static private string ERROR(string restParameters)
        {
            string fail = "Ошибка неизвестного вида";
            int numberOfError = 0;
            try
            {
                numberOfError = Convert.ToInt32(restParameters);
                return serviceCodeToDefinition[numberOfError]();
            }
            catch { }
            return fail;
        }
    }
}
