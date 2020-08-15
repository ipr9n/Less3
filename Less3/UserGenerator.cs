using System;
using System.Collections.Generic;
using Bogus;

namespace Less3
{
    public class UserGenerator
    {
        Random random = new Random();
        delegate string UserTextGenerator();

        readonly UserTextGenerator _getUserText;
        private readonly string _phonePattern;
        private double _mistakeCounter = 0.0d;
        private static string _symbols = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ1234567890";
        private Mistakes _mistakeType;
        private readonly Faker _faker;

        public UserGenerator(string locale)
        {
            _getUserText = GetRusEngUser;

            switch (locale)
            {
                case "ru_RU":
                    _faker = new Faker("ru");
                    _phonePattern = "# (###) ###-##-##";
                    break;


                case "en_US":
                    _faker = new Faker("en_US");
                    _phonePattern = "+# (###) ###-####";
                    _symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                    break;

                default:
                    _faker = new Faker("ru");
                    _phonePattern = "+375 29 ###-##-##";
                    _getUserText = GetBelarussianUser;
                    break;
            }
        }
        private string GetBelarussianUser() => $"{BelarussianData.names[random.Next(BelarussianData.names.Length - 1)]} " +
                      $"{BelarussianData.lastNames[random.Next(BelarussianData.lastNames.Length - 1)]} " +
                      $"{BelarussianData.patronymics[random.Next(BelarussianData.patronymics.Length - 1)]};" +
                      $"{_faker.Address.ZipCode()}," +
                      $"г.{BelarussianData.cityes[random.Next(BelarussianData.cityes.Length - 1)]}," +
                      $"вул.{BelarussianData.streets[random.Next(BelarussianData.streets.Length - 1)]} д.{random.Next(1, 100)},кв.{random.Next(1, 100)};" +
                      $"{_faker.Phone.PhoneNumber(_phonePattern)}";

        private string GetRusEngUser() => ($"{_faker.Name.FullName()};" +
                            $"{_faker.Address.ZipCode()}," +
                            $"{_faker.Address.City()}," +
                            $" {_faker.Address.StreetAddress(true)};" +
                            $" {_faker.Phone.PhoneNumber(_phonePattern)}");

        public List<String> GenerateUser(int count, double mistakeCount = 0.0d)
        {
            List<String> outputList = new List<string>();
            _mistakeCounter = mistakeCount;

            for (var x = 0; x < count; x++)
            {
                var text = _getUserText();
                MakeMistake(ref text, mistakeCount);
                outputList.Add(text);
            }

            return outputList;
        }

        private void MakeMistake(ref string text, double mistakeCount)
        {
            while (_mistakeCounter >= 1)
            {
                var randomIndex = random.Next(0, text.Length - 1);

                switch (_mistakeType)
                {
                    case Mistakes.Switch:
                        _mistakeType = Mistakes.AddChar;
                        var chararray = text.ToCharArray();
                        Array.Reverse(chararray, randomIndex, 2);
                        text = new string(chararray);
                        break;

                    case Mistakes.AddChar:
                        text = text.Insert(randomIndex, $"{_symbols[random.Next(0, _symbols.Length)]}");
                        _mistakeType = Mistakes.RemoveChar;
                        break;

                    case Mistakes.RemoveChar:
                        text = text.Remove(randomIndex, 1);
                        _mistakeType = Mistakes.Switch;
                        break;
                }
                _mistakeCounter--;
            }
            _mistakeCounter += mistakeCount;
        }

        private enum Mistakes
        {
            AddChar,
            RemoveChar,
            Switch
        }
    }
}
