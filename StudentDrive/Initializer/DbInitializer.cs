namespace Initializer
{
    using Core;
    using Data;
    using Data.Utils;
    using System;
    using System.Data.Entity;
    using System.Linq;
    class DbInitializer : DropCreateDatabaseAlways<SDContext>
    {
        protected override void Seed(SDContext db)
        {
            HashString hash = new HashString();
            db.Users.Add(new User { Id = Guid.NewGuid(), FirstName = "Масленников", SecondName = "Сергей", Login = "Sergey", Password = HashString.GetHashString("123") });
            db.Users.Add(new User { Id = Guid.NewGuid(), FirstName = "Бальзамов", SecondName = "Александр", Login = "Alex", Password = HashString.GetHashString("123") });
            db.Statistics.Add(new Statistics(0, 0, 0, db.Users.Local.First(st => st.Login == "Sergey").Id));
            db.Statistics.Add(new Statistics(0, 0, 0, db.Users.Local.First(st => st.Login == "Alex").Id));
            base.Seed(db);
        }
    }
}