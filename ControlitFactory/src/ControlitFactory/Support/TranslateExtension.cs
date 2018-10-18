using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ControlitFactory.Support
{
    /// <summary>
    /// Implementations of this interface MUST convert iOS and Android
    /// platform-specific locales to a value supported in .NET because
    /// ONLY valid .NET cultures can have their RESX resources loaded and used.
    /// </summary>
    /// <remarks>
    /// Lists of valid .NET cultures can be found here:
    ///   http://www.localeplanet.com/dotnet/
    ///   http://www.csharp-examples.net/culture-names/
    /// You should always test all the locales implemented in your application.
    /// </remarks>
    public interface ILocalize
    {
        /// <summary>
        /// This method must evaluate platform-specific locale settings
        /// and convert them (when necessary) to a valid .NET locale.
        /// </summary>
        CultureInfo GetCurrentCultureInfo();

        /// <summary>
        /// CurrentCulture and CurrentUICulture must be set in the platform project, 
        /// because the Thread object can't be accessed in a PCL.
        /// </summary>
        void SetLocale(CultureInfo ci);
    }

    /// <summary>
    /// Helper class for splitting locales like
    ///   iOS: ms_MY, gsw_CH
    ///   Android: in-ID
    /// into parts so we can create a .NET culture (or fallback culture)
    /// </summary>
    public class PlatformCulture
    {
        public PlatformCulture(string platformCultureString)
        {
            if (String.IsNullOrEmpty(platformCultureString))
                throw new ArgumentException("Expected culture identifier", "platformCultureString"); // in C# 6 use nameof(platformCultureString)

            PlatformString = platformCultureString.Replace("_", "-"); // .NET expects dash, not underscore
            var dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);
            if (dashIndex > 0)
            {
                var parts = PlatformString.Split('-');
                LanguageCode = parts[0];
                LocaleCode = parts[1];
            }
            else
            {
                LanguageCode = PlatformString;
                LocaleCode = "";
            }
        }
        public string PlatformString { get; private set; }
        public string LanguageCode { get; private set; }
        public string LocaleCode { get; private set; }
        public override string ToString()
        {
            return PlatformString;
        }
    }

    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci = null;
        readonly string _language = "EN";
        private Dictionary<string, Translations> tulkojumi;
        const string ResourceId = "ControlitFactory.Resx.AppResources";

        public TranslateExtension()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                try
                {
                    var settingi = App.Database.GetProfile().Result;
                    var valoda = "EN";
                    if (settingi.Count > 0)
                    {
                        valoda = settingi[0].Valoda;
                    }
                    _language = valoda;

                }
                catch (Exception ex)
                {
                    if (ex != null)
                    {

                    }
                }
            }
        }

        public TranslateExtension(string language)
        {
            _language = language;
        }
        public string Text { get; set; }

        public string GetTranslation(string text)
        {

            var translation = Tulkojumi[text].GetTranslation(_language);
            if (string.IsNullOrEmpty(translation))
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";


            var translation = Tulkojumi[Text].GetTranslation(_language);
            if (string.IsNullOrEmpty(translation))
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }


        public Dictionary<string, Translations> Tulkojumi
        {
            get
            {
                if (tulkojumi == null)
                {
                    tulkojumi = new Dictionary<string, Translations>();
                    tulkojumi.Add("ActNumberLabel", new Translations("ActNumberLabel",
                        "Akta numurs",
                        "Act number",
                        "Ärendenummer",
                        "Akto numeris",
                        ""));
                    tulkojumi.Add("AddressLabel", new Translations("AddressLabel",
                        "Adrese",
                        "Address",
                        "Adress",
                        "Adresas",
                        ""));
                    tulkojumi.Add("CurrencyLabel", new Translations("CurrencyLabel",
                        "Valūta (saīsinājums)",
                        "Currency abbreviation",
                        "Valuta",
                        "Valiutos simbolis",
                        ""));
                    tulkojumi.Add("DefectPictureLabel", new Translations("DefectPictureLabel",
                        "Defekta attēls",
                        "Defect picture",
                        "Skadebild",
                        "Defekto nuotrauka",
                        ""));
                    tulkojumi.Add("DefektacijasAktsTitle", new Translations("DefektacijasAktsTitle",
                        "Defektācijas akts",
                        "Defective act",
                        "Skadenamn",
                        "Defektinis aktas",
                        ""));
                    tulkojumi.Add("DefektaNrLabel", new Translations("DefektaNrLabel",
                        "Defekts numur",
                        "Defect number",
                        "Skadenummer",
                        "Defekto numeris",
                        ""));
                    tulkojumi.Add("DefektācijasAprakstsLabel", new Translations("DefektācijasAprakstsLabel",
                        "Defektācijas apraksts",
                        "Comments",
                        "Beskrivning",
                        "Komentarai",
                        ""));
                    tulkojumi.Add("DefektiLabel", new Translations("DefektiLabel",
                        "Defekti",
                        "Defects",
                        "Skada",
                        "Defektai",
                        ""));
                    tulkojumi.Add("DefektiTitle", new Translations("DefektiTitle",
                        "Defekti",
                        "Defects",
                        "Skada",
                        "Defektai",
                        ""));
                    tulkojumi.Add("DeleteConfirmationLabel", new Translations("DeleteConfirmationLabel",
                        "Vai tiešām vēlaties dzēst šo ierakstu?",
                        "Do you really want to delete this entry?",
                        "Vill du verkligen ta bort denna text?",
                        "Ar tikrai norite pašalinti šį įrašą?",
                        ""));
                    tulkojumi.Add("DiagnostikaLabel", new Translations("DiagnostikaLabel",
                        "Diagnostika",
                        "Diagnostics",
                        "Diagnos",
                        "Diagnostika",
                        ""));
                    tulkojumi.Add("EmailSubjectLabel", new Translations("EmailSubjectLabel",
                        "Uzģenerētais akts un defektu attēli pielikumā.",
                        "Defective act and defect pictures are in attachment.",
                        "Genererade felbeskrivning och bilderna är bifogade.",
                        "Defektinis aktas ir defektų nuotraukos pateiktos prisegtuke",
                        ""));
                    tulkojumi.Add("epastaTeksts", new Translations("epastaTeksts",
                        "<p><b>Akta nr. {DefektacijasAkts.AktaNr}</b></p>",
                        "<p><b>Act number  {DefektacijasAkts.AktaNr}</b></p>",
                        "<p><b>Act number {DefektacijasAkts.AktaNr}</b></p>",
                        "<p><b>Act number {DefektacijasAkts.AktaNr}</b></p>",
                        ""));
                    tulkojumi.Add("EquipmentCallibrationLabel", new Translations("EquipmentCallibrationLabel",
                        "Iekartas kalibracija (Kv)",
                        "Calibration of the equipment (Kv)",
                        "Kalibrering av utrustningen (Kv)",
                        "Įrangos kalibracija (Kv)",
                        ""));
                    tulkojumi.Add("EquipmentLabel", new Translations("EquipmentLabel",
                        "Iekārtas nosaukums",
                        "Equipment name",
                        "Utrusningens namn",
                        "Įrangos pavadinimas",
                        ""));
                    tulkojumi.Add("EquipmentMenu", new Translations("EquipmentMenu",
                        "Iekārtas",
                        "Equipment",
                        "Utrustning",
                        "Įranga",
                        ""));
                    tulkojumi.Add("HelpMenu", new Translations("HelpMenu",
                        "Palīdzība",
                        "Help",
                        "Hjälp",
                        "Pagalba",
                        ""));
                    tulkojumi.Add("HighWoltageLabel", new Translations("HighWoltageLabel",
                        "Augsta sprieguma pārbaudes iekārta",
                        "High woltage inspection device",
                        "Högspänningsutrutning",
                        "Aukštos įtampos patikros įrenginys",
                        ""));
                    tulkojumi.Add("HorizantalaVirsmaLabel", new Translations("HorizantalaVirsmaLabel",
                        "Horizontālā virsma",
                        "Horizontal surface",
                        "Horisontell yta",
                        "Horizontalus paviršius",
                        ""));
                    tulkojumi.Add("YesLabel", new Translations("YesLabel",
                        "Jā",
                        "Yes",
                        "Ja",
                        "Taip",
                        ""));
                    tulkojumi.Add("KopaLabel", new Translations("KopaLabel",
                        "Kopā",
                        "Total",
                        "Totalt",
                        "Viso",
                        ""));
                    tulkojumi.Add("LaiksKopaLabel", new Translations("LaiksKopaLabel",
                        "Laiks kopā",
                        "Total time",
                        "Total tid",
                        "Visas laikas",
                        ""));
                    tulkojumi.Add("LanguageLabel", new Translations("LanguageLabel",
                        "Valoda",
                        "Language",
                        "Språk",
                        "Kalba",
                        ""));
                    tulkojumi.Add("LogoLabel", new Translations("LogoLabel",
                        "Logo",
                        "Logo",
                        "Logo",
                        "Logotipas",
                        ""));
                    tulkojumi.Add("LowWoltageLabel", new Translations("LowWoltageLabel",
                        "Zema sprieguma pārbaudes iekārta",
                        "Low woltage inspection device",
                        "Lågspänningsutrustning",
                        "Žemos įtampos patikros įrenginys",
                        ""));
                    tulkojumi.Add("MailLabel", new Translations("MailLabel",
                        "Epasts",
                        "Mail",
                        "E-post",
                        "Elektroninis paštas",
                        ""));
                    tulkojumi.Add("MembraneNameLabel", new Translations("MembraneNameLabel",
                        "Hidroizolacijas  membranas nosaukums",
                        "Waterproofing membrane name",
                        "Namn på tätskiktsmembranet",
                        "Hidroizoliacinės membranos pavadinimas",
                        ""));
                    tulkojumi.Add("MembraneThicknessLabel", new Translations("MembraneThicknessLabel",
                        "Hidroizolacijas  membranas biezums",
                        "Waterproofing membrane thickness",
                        "Tjocklek på tätskiktsmembranet",
                        "Hidroizoliacinės membranos storis",
                        ""));
                    tulkojumi.Add("MembraneTypeLabel", new Translations("MembraneTypeLabel",
                        "Hidroizolacijas  membranas veids",
                        "Waterproofing membrane type",
                        "Typ av tätskiktsmembran",
                        "Hidroizoliacinės membranos tipas",
                        ""));
                    tulkojumi.Add("NameLabel", new Translations("NameLabel",
                        "Vārds",
                        "Name",
                        "Namn",
                        "Vardas",
                        ""));
                    tulkojumi.Add("NoLabel", new Translations("NoLabel",
                        "Nē",
                        "No",
                        "Nej",
                        "Ne",
                        ""));
                    tulkojumi.Add("ParbaudamaPlatibaLabel", new Translations("ParbaudamaPlatibaLabel",
                        "Pārbaudāmā platība",
                        "Verifiable area",
                        "Testad yta",
                        "Tikrinamas plotas",
                        ""));
                    tulkojumi.Add("ParbaudePabeigtaLabel", new Translations("ParbaudePabeigtaLabel",
                        "Pārbaude pabeigta",
                        "Complete time",
                        "Atlikimo laikas/trukmėStopptid",
                        "",
                        ""));
                    tulkojumi.Add("ParbaudeUzsaktaLabel", new Translations("ParbaudeUzsaktaLabel",
                        "Pārbaude uzsākta",
                        "Start time",
                        "Starttid",
                        "Pradžios laikas",
                        ""));
                    tulkojumi.Add("ParbaudiVeicaLabel", new Translations("ParbaudiVeicaLabel",
                        "Pārbaudi veica",
                        "Check was done by",
                        "Testet utfört av",
                        "Patikrą atliko",
                        ""));
                    tulkojumi.Add("PasutitajaParstavisLabel", new Translations("PasutitajaParstavisLabel",
                        "Pasūtītāja pārstāvis",
                        "Customer representative",
                        "Beställningsansvarig",
                        "Užsakovo atstovas",
                        ""));
                    tulkojumi.Add("PhoneLabel", new Translations("PhoneLabel",
                        "Tālrunis",
                        "Phone",
                        "Tel",
                        "Telefonas",
                        ""));
                    tulkojumi.Add("PievienotLbel", new Translations("PievienotLbel",
                        "Pievienot",
                        "Add",
                        "Lägg till",
                        "Pridėti",
                        ""));
                    tulkojumi.Add("QuestionLabel", new Translations("QuestionLabel",
                        "Jautājums",
                        "Question",
                        "Fråga",
                        "Klausimas",
                        ""));
                    tulkojumi.Add("SerialNumberLabel", new Translations("SerialNumberLabel",
                        "Sērijas Nr.",
                        "Serial No",
                        "Serienummer",
                        "Serijinis nr.",
                        ""));
                    tulkojumi.Add("SertificateNumberLabel", new Translations("SertificateNumberLabel",
                        "Sertifikāta numurs",
                        "Sertificate number",
                        "Certifikatnummer",
                        "Sertifikato numeris",
                        ""));
                    tulkojumi.Add("SettingsMenu", new Translations("SettingsMenu",
                        "Profils",
                        "Settings",
                        "Inställningar",
                        "Nustatymai",
                        ""));
                    tulkojumi.Add("SurnameLabel", new Translations("SurnameLabel",
                        "Uzvārds",
                        "Surname",
                        "Efternamn",
                        "Pavardė",
                        ""));
                    tulkojumi.Add("TransportaIzdevumiLabel", new Translations("TransportaIzdevumiLabel",
                        "Transporta izdevumi",
                        "Transport costs",
                        "Transportkostnader",
                        "Transporto išlaidos",
                        ""));
                    tulkojumi.Add("UznemumaDatiLabel", new Translations("UznemumaDatiLabel",
                        "Uzņēmuma rekvizīti",
                        "Company info",
                        "Företagsinformation",
                        "Įmonės informacija",
                        ""));
                    tulkojumi.Add("VatLabel", new Translations("VatLabel",
                        "PVN %",
                        "VAT %",
                        "MOMS %",
                        "PVM %",
                        ""));
                    tulkojumi.Add("VerikalaVirsmaLabel", new Translations("VerikalaVirsmaLabel",
                        "Vertikālā virsma (parapets, siena)",
                        "Vertical surface (parapet, wall)",
                        "Vertikal yta (parapet, vägg)",
                        "Vertikalus paviršius (parapetas, siena)",
                        ""));
                }
                return tulkojumi;
            }
        }
    }

    public class Translations
    {
        public Translations(string key, string lv, string en, string se, string lt, string ru)
        {
            Key = key;
            LV = lv;
            EN = en;
            SE = se;
            LT = lt;
            RU = ru;
        }

        public string GetTranslation(string language)
        {
            switch (language)
            {
                case "LV":
                    return LV;
                case "EN":
                    return EN;
                case "SE":
                    return SE;
                case "LT":
                    return LT;
                case "RU":
                    return RU;

                default:
                    return EN;
            }
        }

        public string Key { get; set; }
        public string LV { get; set; }
        public string EN { get; set; }
        public string SE { get; set; }
        public string LT { get; set; }
        public string RU { get; set; }


    }
}
