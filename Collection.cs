using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixGraphicsGenerator
{
    public class Collection
    {
        readonly string filePath;

        public List<MatrixData> Graphics { get; }

        public Collection(string path)
        {
            Graphics = new List<MatrixData>();
            filePath = path;

            if (!File.Exists(filePath))
            {
                Save();
                return;
            }

            string jsonString = File.ReadAllText(filePath);
            Graphics = JsonConvert.DeserializeObject<List<MatrixData>>(jsonString);
            Graphics.Sort((x, y) => x.Name.CompareTo(y.Name));
        }

        /// <summary>
        /// Adds a graphic to the list or overwrites if one is present with the same name
        /// (Used instead of Graphics.Add())
        /// </summary>
        public void AddGraphic(MatrixData graphic)
        {
            int existingDataIndex = Graphics.FindIndex(x => x.Name == graphic.Name);

            if (existingDataIndex != -1)
                Graphics[existingDataIndex] = graphic;
            else
                Graphics.Add(graphic);

            Graphics.Sort((x, y) => x.Name.CompareTo(y.Name));
            Save();
        }

        public void Save()
        {
            string jsonString = JsonConvert.SerializeObject(Graphics, Formatting.Indented);
            File.WriteAllText(filePath, jsonString);
        }

        public void Export(string path)
        {
            int line = 0;
            string[] lines = new string[Graphics.Count + 1];

            lines[line] = "const int GRAPHICS_COUNT = " + Graphics.Count + ";";
            line++;
            for (int i = 0; i < Graphics.Count; i++, line++)
            {
                lines[line] = "const int " + Graphics[i].Name.Replace(" ", string.Empty) + "[" + Graphics[i].Size.Item2 + "] = { " + Graphics[i].Code + " };";
            }

            File.WriteAllLines(path, lines);
        }
    }

    /// <summary>
    /// Contains needed information for saving a graphic
    /// </summary>
    public struct MatrixData
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public List<int> RowCodes { get; private set; }
        public Tuple<int, int> Size { get; private set; }

        public MatrixData(string name, string code, List<int> rowCodes, Tuple<int, int> size)
        {
            Name = name;
            Code = code;
            RowCodes = rowCodes;
            Size = size;
        }
    }
}
