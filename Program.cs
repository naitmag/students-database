using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Database
{
    internal class Program
    {
        static void Main(string[] args)
        {

            StudBase studbase = DeserializeObject<StudBase>("data.bin");


            while (true)
            {
                Console.WriteLine("==========База данных студентов ФКиСКД==========\n");
                Console.WriteLine("Выберите действие:\n1)Добавить студента\n2)Найти студента\n3)Удалить студента\n4)Показать всех студентов\n5)Выход");



                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        CreateStudent(studbase);
                        break;
                    case ConsoleKey.D2:
                        FindStudent(studbase);
                        break;
                    case ConsoleKey.D3:
                        Remove(studbase);
                        break;
                    case ConsoleKey.D4:
                        PrintStudentsList(studbase, studbase.GetList(), true);
                        break;
                    case ConsoleKey.F12:
                        SerializeObject("data.bin", studbase);
                        Console.WriteLine("Данные сохранены");
                        break;

                }
                Console.Clear();
            }


        }
        static void Remove(StudBase studbase)
        {
            string id;
            Console.WriteLine("Введите ID студента:");
            id = Console.ReadLine();
            studbase.RemoveStudent(Guid.Parse(id));
            Console.Clear();
            Console.WriteLine("Студент удален.");
        }
        static void FindStudent(StudBase studbase)
        {
            Console.Clear();

            string request;
            Console.WriteLine("Поиск:\n1)По фамилии\n2)По имени\n3)По группе\n4)По месту жительства\n5)По инстаграму\n6)По телеграму");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("Введите фамилию");
                    request = Console.ReadLine();
                    PrintStudentsList(studbase, studbase.FindByLastName(request), false);
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine("Введите имя");
                    request = Console.ReadLine();
                    PrintStudentsList(studbase, studbase.FindByName(request), false);
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("Введите группу");
                    request = Console.ReadLine();
                    PrintStudentsList(studbase, studbase.FindByGroup(request), false);
                    break;
                case ConsoleKey.D4:
                    Console.WriteLine("Введите название населенного пункта");
                    request = Console.ReadLine();
                    PrintStudentsList(studbase, studbase.FindByLocation(request), false);
                    break;
                case ConsoleKey.D5:
                    Console.WriteLine("Введите инстаграм");
                    request = Console.ReadLine();
                    PrintStudentsList(studbase, studbase.FindByInst(request), false);
                    break;
                case ConsoleKey.D6:
                    Console.WriteLine("Введите инстаграм");
                    request = Console.ReadLine();
                    PrintStudentsList(studbase, studbase.FindByTelegram(request), false);
                    break;
            }

        }
        static void CreateStudent(StudBase database)
        {
            string fullName;
            string group;
            bool gender;

            Console.WriteLine("Введите ФИО Студента:");
            fullName = Console.ReadLine();
            if (fullName.Split(' ').Count() < 2)
            {
                Console.WriteLine("Неверные данные");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Введите группу студента:");
            group = Console.ReadLine();
            Console.WriteLine("Укажите пол (по умолчанию мужской):");
            Console.WriteLine("0 - мужской, 1 - женский");
            Boolean.TryParse(Console.ReadLine(), out gender);
            database.Push(new Student(fullName, group, gender));
            Console.Clear();
            Console.WriteLine("Студент создан.");

        }

        static void SerializeObject<T>(string fileName, T obj)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
            }
        }

        static T DeserializeObject<T>(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fs);
            }
        }

        static void EditStudentInfo(Student student)
        {
            Console.Clear();
            Console.WriteLine("Изменить:\n[1]ФИО\n[2]Группу\n[3]Место проживания\n[4]Пол\n");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("\nВведите новое ФИО:");
                    student.EditFullName(Console.ReadLine());
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine("\nВведите новую группу:");
                    student.EditGroup(Console.ReadLine());
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("\nВведите место жительства:");
                    student.EditLocation(Console.ReadLine());
                    break;
                case ConsoleKey.D4:
                    student.EditGender(GetGender());
                    break;
            }
        }
        static bool GetGender()
        {
            Console.Clear();
            bool isFemale = false;
            bool getAnswer = false;
            ConsoleColor oldColor = Console.ForegroundColor;


            while (!getAnswer)
            {
                Console.Clear();
                Console.WriteLine("Укажите пол:\n");

                Console.ForegroundColor = isFemale ? oldColor : ConsoleColor.Blue;
                Console.Write("Мужской\t\t");
                Console.ForegroundColor = isFemale ? ConsoleColor.Magenta : oldColor;
                Console.WriteLine("Женский");
                Console.SetCursorPosition(isFemale ? 16 : 0, 3);
                Console.ForegroundColor = isFemale ? ConsoleColor.Magenta : ConsoleColor.Blue;
                Console.WriteLine("=======");
                Console.ForegroundColor = oldColor;
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.RightArrow:
                        isFemale = true;
                        break;
                    case ConsoleKey.LeftArrow:
                        isFemale = false;
                        break;
                    case ConsoleKey.Enter:
                        getAnswer = true;
                        break;
                }
            }

            return isFemale;
        }
        static void GetStudentMenu(StudBase studentBase, Student student)
        {
            ConsoleColor color = ConsoleColor.Magenta;
            ConsoleColor oldColor = Console.ForegroundColor;

            bool getTargetKey = false;
            while (!getTargetKey)
            {
                Console.Clear();
                Console.ForegroundColor = color;
                Console.WriteLine("===================");
                Console.ForegroundColor = oldColor;

                student.ShowFullInfo();

                Console.ForegroundColor = color;
                Console.WriteLine("===================");
                Console.ForegroundColor = oldColor;

                Console.WriteLine("Выберите действие:\n1)Изменить студента\n2)Удалить студента");


                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Escape:
                        getTargetKey = true;
                        break;
                    case ConsoleKey.D1:
                        EditStudentInfo(student);
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("Вы уверены?");
                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                        {
                            studentBase.RemoveStudent(student.ID);
                            getTargetKey = true;
                        }
                        break;

                }
            }

        }

        static void PrintStudentsList(StudBase studentBase, List<Student> list, bool showFullInfo)
        {

            ConsoleColor color = ConsoleColor.Blue;
            ConsoleColor oldColor = Console.ForegroundColor;
            int resultsOnPage = 3;
            int maxPages;
            int targetPage = 0;

            bool closeList = false;

            var studentsOnScreen = new List<Student>();
            maxPages = list.Count / resultsOnPage + (list.Count % resultsOnPage > 0 ? 1 : 0) - 1;


            while (!closeList)
            {
                Console.Clear();
                if (list.Count == 0)
                {
                    Console.WriteLine("Нет результатов");
                    Console.ReadKey();
                    return;
                }
                if (targetPage > maxPages)
                {
                    targetPage = maxPages;
                    Console.WriteLine("Достигнута последняя страница");
                }
                else if (targetPage < 0)
                {
                    targetPage = 0;
                    Console.WriteLine("Достигнута первая страница");
                }
                Console.WriteLine("Результатов:" + list.Count());
                for (int j = targetPage * resultsOnPage; j < resultsOnPage * targetPage + resultsOnPage; j++)
                {
                    if (j >= list.Count)
                    {
                        break;
                    }
                    Console.ForegroundColor = color;
                    Console.WriteLine("=================");
                    Console.ForegroundColor = oldColor;
                    list[j].ShowInfo();
                    Console.ForegroundColor = color;
                    Console.WriteLine("=================");
                    Console.ForegroundColor = oldColor;
                    studentsOnScreen.Add(list[j]);
                }

                Console.WriteLine($"Страницы {targetPage + 1} из {maxPages + 1}");

                bool getTargetKey = false;
                while (!getTargetKey)
                {
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.RightArrow:
                            targetPage++;
                            getTargetKey = true;
                            break;
                        case ConsoleKey.LeftArrow:
                            targetPage--;
                            getTargetKey = true;
                            break;
                        case ConsoleKey.D1:
                            GetStudentMenu(studentBase, studentsOnScreen[0]);
                            getTargetKey = true;
                            break;
                        case ConsoleKey.D2:
                            GetStudentMenu(studentBase, studentsOnScreen[1]);
                            getTargetKey = true;
                            break;
                        case ConsoleKey.Escape:
                            getTargetKey = true;
                            closeList = true;
                            break;
                    }
                }
                studentsOnScreen.Clear();

            }


        }
    }
    [Serializable]
    class Student
    {
        private string _firstName;
        private string _lastName;
        private string _surname;

        private bool _gender;

        private string _group;

        private string _location;

        private string _note;

        private Guid _id;

        private string _inst;
        private string _telegram;

        public string FirstName { get { return _firstName; } }
        public string LastName { get { return _lastName; } }
        public string Surname { get { return _surname; } }
        public string Group { get { return _group; } }
        public string Location { get { return _location; } }
        public string Inst { get { return _inst; } }
        public string Telegram { get { return _telegram; } }
        public Guid ID { get { return _id; } }

        public Student(string fullName, string group, bool gender = false)
        {
            string[] args = fullName.Split(' ');
            _lastName = args[0];
            _firstName = args[1];
            _surname = args.Length > 2 ? args[2] : "";

            _group = group;

            _gender = gender;

            _id = Guid.NewGuid();

        }

        public void ShowInfo()
        {
            Console.WriteLine($"Фио: {_lastName} {_firstName} {_surname}");
            Console.WriteLine($"Группа: {_group}");
            Console.WriteLine($"ID: {_id}");
        }
        public void ShowFullInfo()
        {
            Console.WriteLine($"Фио: {_lastName} {_firstName} {_surname}\nПол: " + (_gender ? "Женский" : "Мужской") + $"\nГруппа: {_group}\nID:{_id}\nInst: {_inst}\nTelegram:{_telegram}\nМесто жительства: {_location}\nПримечание: {_note}");

        }

        public void EditFullName(string fullName)
        {
            string[] args = fullName.Split(' ');
            _lastName = args[0];
            _firstName = args[1];
            _surname = args.Length > 2 ? args[2] : "";
        }
        public void EditGroup(string group)
        {
            _group = group;
        }
        public void EditLocation(string location)
        {
            _location = location;
        }
        public void EditGender(bool isFemale)
        {
            _gender = isFemale;
        }
    }
    [Serializable]
    class StudBase
    {
        private List<Student> _students = new List<Student>();

        public void Push(Student student)
        {
            _students.Add(student);

        }

        public List<Student> FindByName(string name)
        {
            List<Student> result = new List<Student>();
            foreach (var student in _students)
            {
                if (student.FirstName.ToLower().Contains(name.ToLower()))
                {
                    result.Add(student);
                }
            }
            return result;
        }
        public List<Student> FindByLastName(string lastName)
        {
            List<Student> result = new List<Student>();
            foreach (var student in _students)
            {
                if (student.LastName.ToLower().Contains(lastName.ToLower()))
                {
                    result.Add(student);
                }
            }
            return result;
        }
        public List<Student> FindByGroup(string group)
        {
            List<Student> result = new List<Student>();
            foreach (var student in _students)
            {
                if (student.Group.ToLower().Contains(group.ToLower()))
                {
                    result.Add(student);
                }
            }
            return result;
        }
        public List<Student> FindByLocation(string location)
        {
            List<Student> result = new List<Student>();
            foreach (var student in _students)
            {
                if (student.Location.ToLower().Contains(location.ToLower()))
                {
                    result.Add(student);
                }
            }
            return result;
        }
        public List<Student> FindByInst(string inst)
        {
            List<Student> result = new List<Student>();
            foreach (var student in _students)
            {
                if (student.Inst.Contains(inst))
                {
                    result.Add(student);
                }
            }
            return result;
        }
        public List<Student> FindByTelegram(string telegram)
        {
            List<Student> result = new List<Student>();
            foreach (var student in _students)
            {
                if (student.Inst.Contains(telegram))
                {
                    result.Add(student);
                }
            }
            return result;
        }
        public void RemoveStudent(Guid id)
        {
            for (int i = 0; i < _students.Count; i++)
            {
                if (_students[i].ID == id)
                {
                    _students.RemoveAt(i);
                    break;
                }
            }
        }
        public List<Student> GetList()
        {
            return _students;
        }

    }
}
