using System;
using System.Collections.Generic;
using System.IO;
using Muldis.DBP;

[assembly: CLSCompliant(true)]

namespace Muldis.D.Ref_Eng.Console
{
    public class Program
    {
        public static void Main(String[] args)
        {
            if (args.Length != 1)
            {
                // TODO: Review whether host_executable_path reflects actual
                // main program invocation syntax or not, and accounts for
                // the varied host operating systems or compiled vs debugged.
                String host_executable_path
                    = System.Reflection.Assembly.GetEntryAssembly().Location;
                System.Console.WriteLine(
                    "Usage: " + host_executable_path + " <source_code_file_path>");
                return;
            }

            // File system path for the file containing plain text source
            // code that the user wishes to execute as their main program.
            String source_code_file_path = args[0];

            // If the user-specified file path is absolute, Path.Combine()
            // will just use that as the final path; otherwise it is taken
            // as relative to the host executable's current working directory.
            source_code_file_path = Path.Combine(
                Directory.GetCurrentDirectory(), source_code_file_path);

            if (!File.Exists(source_code_file_path))
            {
                System.Console.WriteLine("The requested source code providing file"
                    + " [" + source_code_file_path + "] doesn't exist.");
                return;
            }

            Byte[] source_code_file_content;
            try
            {
                source_code_file_content = File.ReadAllBytes(source_code_file_path);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("The requested source code providing file"
                    + " [" + source_code_file_path + "] couldn't be read:"
                    + " " + e.ToString());
                return;
            }

            // Instantiate object of a Muldis DataBase Protocol provider information class.
            IInfo provider_info = new Muldis.D.Ref_Eng.Info();

            // Request a VM object implementing a specific version of the MDBP or
            // what the info provider considers the next best fit version;
            // this would die if it thinks it can't satisfy an acceptable version.
            // We will use this for all the main work.
            IMachine machine = provider_info.Want_VM_API(
                new String[] {"Muldis_DBP_DotNet", "http://muldis.com", "0.1.0.-9"});
            if (machine == null)
            {
                System.Console.WriteLine(
                    "The requested Muldis DataBase Protocol provider"
                    + " information class [Muldis.D.Ref_Eng.Info]"
                    + " doesn't provide the specific MDBP version needed.");
                return;
            }

            // Import the user-specified source code file's raw content into
            // the MDBP-implementing virtual machine where it would be used.
            IMD_Blob source_code_blob = machine.Importer().MD_Blob(source_code_file_content);

            // Temporary Executor test.
            IImporter i = machine.Importer();
            IMD_Integer sum = (IMD_Integer)machine.Executor().Evaluates(
                i.MD_Attr_Name_List(new String[] {"foundation", "Integer_plus"}),
                i.MD_Tuple(27,39)
            );
            IMD_Tuple that = i.MD_Tuple(27,39);
            IMD_Tuple that_too = i.MD_Tuple(attrs: new Dictionary<String,Object>()
                {{"\u0014", 25}, {"aye", "zwei"}, {"some one", "other two"}}
            );
            IMD_Text the_other = i.MD_Text("Fr ⊂ ac 💩 ti ÷ on");
            IMD_Fraction f0 = i.MD_Fraction(014.0M);
            IMD_Fraction f1 = i.MD_Fraction(2.3M);
            IMD_Fraction f2 = i.MD_Fraction(02340233.23402532000M);
            IMD_Fraction f3 = i.MD_Fraction(13,5);
            IMD_Fraction f4 = i.MD_Fraction(27,6);
            IMD_Fraction f5 = i.MD_Fraction(35,-41);
            IMD_Fraction f6 = i.MD_Fraction(-54235435432,32543252);
            IMD_Fraction f7 = i.MD_Fraction(26,13);
            IMD_Fraction f8 = i.MD_Fraction(5,1);
            IMD_Fraction f9 = i.MD_Fraction(5,-1);
        }
    }
}
