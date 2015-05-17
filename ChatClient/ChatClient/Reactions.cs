using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public static class Reactions
    {
        static private Dictionary<int, Func<string>> serviceCodeToDefinition = new Dictionary<int, Func<string>>();
        static private Dictionary<string, Func<string, string>> commandToHandler = new Dictionary<string, Func<string, string>>();
        static Reactions()
        {
            serviceCodeToDefinition.Add(1, ErrorIncorrectFormatOfMessage);
            serviceCodeToDefinition.Add(2, UnknownErrorOfSendingMessage);
            serviceCodeToDefinition.Add(3, ErrorUnexistingUser);
            serviceCodeToDefinition.Add(50, NickHasChangedSuccesful);
            serviceCodeToDefinition.Add(51, ErrorTheNickIsUsed);
            serviceCodeToDefinition.Add(52, ErrorTheNickIsIncorrect);
            serviceCodeToDefinition.Add(53, NickWasSuccessfullyRegistered);
            serviceCodeToDefinition.Add(54, UnregisteredNickOrInvalidPassword);
            serviceCodeToDefinition.Add(55, LogInWasSuccessfully);
            serviceCodeToDefinition.Add(100, ErrorServerIsGoingToStop);
            //*****************************//
            commandToHandler.Add("MSG", MSG);
            commandToHandler.Add("IAMSERV", IAMSERV);
            commandToHandler.Add("ERROR", ERROR);
            commandToHandler.Add("PRIVMSG", PRIVMSG);
            commandToHandler.Add("NAMES", NAMES);
            commandToHandler.Add("YOUARE", YOUARE);
        }
        static private string LogInWasSuccessfully()
        {
            return "Успешная авторизация.";
        }
        static private string UnregisteredNickOrInvalidPassword()
        {
            //Client.GetInstance().ownNickName = "";
            return "Неверный пароль или логин.";
        }
        static private string NickWasSuccessfullyRegistered()
        {
            return "Ник был удачно зарегестрирован.";
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
            return "Данный ник-нейм не соответствует правилам сервера.";
        }
        static private string ErrorServerIsGoingToStop()
        {
            return "Сервер собирается приостановить свою работу. Будет произведен разрыв соединения.";
        }

        //*****************************//

        static private string MSG(string restParameters)
        {
            return (restParameters);
        }
        static private string PRIVMSG(string restParameters)
        {
            int indexForDivision = restParameters.IndexOf(' ');
            restParameters = restParameters.Insert(indexForDivision, ":");
            return ("Сообщение от " + restParameters);
        }
        static private string ERROR(string restParameters)
        {
            string UndefinedError = "Ошибка неизвестного вида.";
            int numberOfError = 0;
            try
            {
                numberOfError = Convert.ToInt32(restParameters);
                return serviceCodeToDefinition[numberOfError]();
            }
            catch { }
            return UndefinedError;
        }
        static private string YOUARE(string restParameters)
        {
            return "Сервер Вас приветствует, " + restParameters + " !";
        }
        static private string IAMSERV(string restParameters)
        {
            //Обновление параметров
            return ("Сервер представлен под именем + " + restParameters + ".");
        }
        static private string NAMES(string restParameters)
        {
            return "Обновление списка пользователей.";
        }

        public static Func<string, string> GetCommandHandler(string command)
        {
            return commandToHandler[command];
        }
        public static bool ContainsHandlerForCommand(string command)
        {
            return commandToHandler.ContainsKey(command);
        }
    }
}
