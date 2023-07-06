using System;
using System.IO;
using System.Linq;
class Program
{
    static void Main()
    {
        // Step 1: Read the text file
        string textFilePath = "C:\\Users\\rachanap\\Documents\\notepad to csv\\loadinitlog_20230704_094258.log";
        string[] allLines = File.ReadAllLines(textFilePath);

        // Step 2: Find start and end line indices
        int startLineIndex = -1;
        int endLineIndex = -1;

        for (int i = 0; i < allLines.Length; i++)
        {
            if (allLines[i].Contains("iC_tVerify Statistics: Per Test Instance [BEGIN]"))
            {
                startLineIndex = i + 1;
            }
            else if (allLines[i].Contains("iC_tVerify Statistics: Per Test Instance [FINISH]"))
            {
                endLineIndex = i;
                break;
            }
        }

        if (startLineIndex == -1 || endLineIndex == -1)
        {
            Console.WriteLine("Start or end line not found.");
            return;
        }

        string[] extractedLines = new string[endLineIndex - startLineIndex - 1];
        Array.Copy(allLines, startLineIndex + 1, extractedLines, 0, extractedLines.Length);

        // Step 3: Remove row 2 from the extracted lines
        extractedLines = extractedLines.Where((line, index) => index != 0).ToArray();

        // Step 4: Write the extracted lines to a CSV file
        string csvFilePath = "C:\\Users\\rachanap\\Documents\\notepad to csv\\output_final_1.csv";
        using (StreamWriter writer = new StreamWriter(csvFilePath))
        {
            // Define the column names
            string[] columnNames = { "Index", "Private_Bytes", "Virtual_bytes", "Verify_time", "Test_Class", "Instance_Name" };

            // Write the column names to the CSV file
            writer.WriteLine(string.Join(",", columnNames));

            foreach (string line in extractedLines)
            {
                string[] fields = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] remainingFields = new string[fields.Length - 2];
                Array.Copy(fields, 2, remainingFields, 0, remainingFields.Length);

                writer.WriteLine(string.Join(",", remainingFields));
            }
        }

        Console.WriteLine("CSV file created successfully with row 2 removed.");
    }
}
