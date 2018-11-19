using ControlitFactory.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ControlitFactory.DataManager
{
    public class DataManager
    {


        readonly SQLiteAsyncConnection db;
        public DataManager(string dbPath)
        {
            try
            {
                db = new SQLiteAsyncConnection(dbPath);
                var setings = db.GetConnection().GetTableInfo("Settings");
                if (setings.Count == 0)
                {
                    db.CreateTableAsync<Settings>().Wait();
                }
                else
                {
                    if (!setings.Any(x => x.Name == "Valoda"))
                    {
                        db.ExecuteAsync("ALTER TABLE Settings ADD COLUMN Valoda TEXT").Wait();
                    }
                    if (!setings.Any(x => x.Name == "Logo"))
                    {
                        db.ExecuteAsync("ALTER TABLE Settings ADD COLUMN Logo TEXT").Wait();
                    }
                    if (!setings.Any(x => x.Name == "UznemumaDati"))
                    {
                        db.ExecuteAsync("ALTER TABLE Settings ADD COLUMN UznemumaDati TEXT").Wait();
                    }
                    if (!setings.Any(x => x.Name == "Currency"))
                    {
                        db.ExecuteAsync("ALTER TABLE Settings ADD COLUMN Currency TEXT").Wait();
                    }
                }
                var defaktacijasAkts = db.GetConnection().GetTableInfo("DefektacijasAkts");
                if (defaktacijasAkts.Count == 0)
                {
                    db.CreateTableAsync<DefektacijasAkts>().Wait();
                }
                else
                {
                    if (!defaktacijasAkts.Any(x => x.Name == "LowVoltageEquipmentName"))
                    {
                        db.ExecuteAsync("ALTER TABLE DefektacijasAkts ADD COLUMN LowVoltageEquipmentName TEXT").Wait();
                    }
                    if (!defaktacijasAkts.Any(x => x.Name == "LowVoltageEquipmentSerial"))
                    {
                        db.ExecuteAsync("ALTER TABLE DefektacijasAkts ADD COLUMN LowVoltageEquipmentSerial TEXT").Wait();
                    }
                    if (!defaktacijasAkts.Any(x => x.Name == "HighVoltageEquipmentName"))
                    {
                        db.ExecuteAsync("ALTER TABLE DefektacijasAkts ADD COLUMN HighVoltageEquipmentName TEXT").Wait();
                    }
                    if (!defaktacijasAkts.Any(x => x.Name == "HighVoltageEquipmentSerial"))
                    {
                        db.ExecuteAsync("ALTER TABLE DefektacijasAkts ADD COLUMN HighVoltageEquipmentSerial TEXT").Wait();
                    }
                    if (!defaktacijasAkts.Any(x => x.Name == "IekartasKalibracija"))
                    {
                        db.ExecuteAsync("ALTER TABLE DefektacijasAkts ADD COLUMN IekartasKalibracija TEXT").Wait();
                    }
                    if (!defaktacijasAkts.Any(x => x.Name == "MembranasVeids"))
                    {
                        db.ExecuteAsync("ALTER TABLE DefektacijasAkts ADD COLUMN MembranasVeids TEXT").Wait();
                    }
                    if (!defaktacijasAkts.Any(x => x.Name == "MembranasNosaukums"))
                    {
                        db.ExecuteAsync("ALTER TABLE DefektacijasAkts ADD COLUMN MembranasNosaukums TEXT").Wait();
                    }
                    if (!defaktacijasAkts.Any(x => x.Name == "MembranasBiezums"))
                    {
                        db.ExecuteAsync("ALTER TABLE DefektacijasAkts ADD COLUMN MembranasBiezums TEXT").Wait();
                    }
                }

                var defekts = db.GetConnection().GetTableInfo("Defekts");
                if (defekts.Count == 0)
                {
                    db.CreateTableAsync<Defekts>().Wait();
                }
                var Classifiers = db.GetConnection().GetTableInfo("Classifiers");
                if (Classifiers.Count == 0)
                {
                    db.CreateTableAsync<Classifiers>().Wait();
                }
            }
            catch(Exception ex)
            {

            }
        }

        public Task<List<Settings>> GetProfile()
        {
            return db.Table<Settings>().ToListAsync();
        }

        public void InsertProfile(Settings profile)
        {
            db.ExecuteAsync("DELETE FROM Settings").Wait();
            db.InsertAsync(profile);
        }
        public Task<List<DefektacijasAkts>> GetDefektacijasAkti()
        {
            return db.Table<DefektacijasAkts>().OrderByDescending(x => x.ParbaudeUzsakta).ToListAsync();
        }
        public Task<List<Classifiers>> GetEquipmentList()
        {
            return db.Table<Classifiers>().OrderByDescending(x => x.Code).ToListAsync();
        }

        public DefektacijasAkts GetDefektacijasAkts(int id)
        {
            return db.GetAsync<DefektacijasAkts>(id).Result;
        }
        public Classifiers GetClassifier(int id)
        {
            return db.GetAsync<Classifiers>(id).Result;
        }
        public Defekts GetDefekts(int id)
        {
            return db.GetAsync<Defekts>(id).Result;
        }

        public void InsertDefektacijasAkts(DefektacijasAkts akts)
        {
            try
            {

                if (akts.Id == 0)
                {
                    db.InsertAsync(akts).ContinueWith(t =>
                    {
                        App.AktaId = akts.Id;
                        Console.WriteLine("New ID: {0}", akts.Id);
                    });
                }
                else
                {
                    var sql = @"UPDATE DefektacijasAkts SET
                                AktaNr = @AktaNr,
                                Adrese = @Adrese,
                                Latitude = @Latitude,
                                Longitude = @Longitude,
                                Talrunis = @Talrunis,
                                Epasts = @Epasts,
                                ParbaudamaPlatiba =@ParbaudamaPlatiba,
                                TransportaIzdevumi = TransportaIzdevumi,
                                Diagnostika = @Diagnostika,
                                Kopa = @Kopa,
                                Paraksts = @Paraksts,
                                ParbaudeUzsakta = @ParbaudeUzsakta,
                                ParbaudePabeigta = @ParbaudePabeigta,
                                LaiksKopa = @LaiksKopa,
                                ParbaudiVeica = @ParbaudiVeica,
                                Vat = @Vat,
                                PasutitajaParstavis = @PasutitajaParstavis,
                                ImageFolder = @ImageFolder
                                WHERE Id = @Id
                                ";

                    db.UpdateAsync(akts);
                }
            }
            catch(Exception ex)
            {

            }
        }
        public async Task<int> InsertAkts(DefektacijasAkts akts)
        {
            if (akts.Id == 0)
            {
                var t = db.InsertAsync(akts);
                t.Wait();
                return akts.Id;
            }
            return 0;
        }
        public void InsertDefekts(Defekts defekts)
        {
            if (defekts.Id == 0)
            {
                db.InsertAsync(defekts).ContinueWith(t =>
                {
                    Console.WriteLine("New customer ID: {0}", defekts.Id);
                }); 
            }
            else
            {
                db.UpdateAsync(defekts);
            }
        }
        public void InsertClassifier(Classifiers ieraksts)
        {
            if (ieraksts.Id == 0)
            {
                db.InsertAsync(ieraksts).ContinueWith(t =>
                {
                    Console.WriteLine("New customer ID: {0}", ieraksts.Id);
                }).Wait();
            }
            else
            {
                db.UpdateAsync(ieraksts).Wait();
            }
        }
        public async Task DeleteClassifier(Classifiers ieraksts)
        {
            await db.DeleteAsync(ieraksts);
        }

        public async Task DeleteDefektacijasAkts(DefektacijasAkts akts)
        {
            await db.DeleteAsync(akts);
        }
        public async Task DeleteDefekts(Defekts defekts)
        {
            await db.DeleteAsync(defekts);
        }


        public Task<List<Defekts>> GetDefekti(int DefektācijasAktaId)
        {
            return db.Table<Defekts>().Where(x => x.DefektacijasAktaId == DefektācijasAktaId).ToListAsync();
        }
        public Task<List<Defekts>> GetDefektiSync(int DefektācijasAktaId)
        {
            return db.Table<Defekts>().Where(x => x.DefektacijasAktaId == DefektācijasAktaId).ToListAsync();
        }

    }
}
