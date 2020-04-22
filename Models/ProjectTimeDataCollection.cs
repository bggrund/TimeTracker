using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TimeTracker
{
    public class ProjectTimeDataCollection
    {
        #region Private Members

        #endregion

        public static List<ProjectTimeData> GetProjectTimeData(string projectTimeDataFilePath)
        {
            List<ProjectTimeData> items;
            if (File.Exists(projectTimeDataFilePath))
            {
                using (FileStream f = new FileStream(projectTimeDataFilePath, FileMode.Open))
                {
                    items = (List<ProjectTimeData>)(new XmlSerializer(typeof(List<ProjectTimeData>))).Deserialize(f);
                }
            }
            else
            {
                //File.Create(projectTimeDataFilePath);
                items = new List<ProjectTimeData>();
            }

            return items;
        }

        public static void SaveProjectTimeData(string projectTimeDataFilePath, List<ProjectTimeData> items)
        {
            using (FileStream f = new FileStream(projectTimeDataFilePath, FileMode.Create))
            {
                (new XmlSerializer(typeof(List<ProjectTimeData>))).Serialize(f, items);
            }
        }

        public static ProgramData GetProgramData(string filePathDataPath)
        {
            ProgramData progData;
            if (File.Exists(filePathDataPath))
            {
                using (FileStream f = new FileStream(filePathDataPath, FileMode.Open))
                {
                    progData = (ProgramData)(new XmlSerializer(typeof(ProgramData))).Deserialize(f);
                }
            }
            else
            {
                //File.Create(projectTimeDataFilePath);
                progData = new ProgramData();
            }

            return progData;
        }

        public static void SaveProgramData(string filePathDataPath, ProgramData progData)
        {
            using (FileStream f = new FileStream(filePathDataPath, FileMode.Create))
            {
                (new XmlSerializer(typeof(ProgramData))).Serialize(f, progData);
            }
        }
    }
    
    public class ProgramData
    {
        public string ProjectTimeDataPath;
        public string TimesheetsDir;
        public string Name;
        public string ID;

        public ProgramData(string projectTimeDataPath, string timesheetsDir, string name, string id)
        {
            ProjectTimeDataPath = projectTimeDataPath;
            TimesheetsDir = timesheetsDir;
            Name = name;
            ID = id;
        }
        public ProgramData()
        {
            ProjectTimeDataPath = TimesheetsDir = Name = ID = "";
        }
    }
}
