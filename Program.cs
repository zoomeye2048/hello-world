using System;
using System.IO;
using System.Globalization;
using System.CodeDom.Compiler;


namespace Compiler
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length>0)
            {
                if (File.Exists(args[0]))
                {
                    CompileExecutable(args[0]);
                }
                else
                {
                    Console.WriteLine("source file not found - {0}", args[0]);
                }
            }
            else
            {
                Console.WriteLine("No such param!");
            }
            //CompileExecutable(@"C:\Users\noder\Desktop\Program.cs");
            Console.ReadKey();
        }

        private static bool CompileExecutable(string sourceName)
        {
            FileInfo sourceFile = new FileInfo(sourceName);
            CodeDomProvider provider = null;
            bool compileOK = false;
            if (sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture)==".CS")
            {
                provider = CodeDomProvider.CreateProvider("CSharp");
            }
            else
            {
                Console.WriteLine("Source file must have a .cs extension");
            }

            if (provider!=null)
            {
                string exeName = string.Format(@"{0}\{1}.exe",System.Environment.CurrentDirectory,sourceFile.Name.Replace(".","_"));
                CompilerParameters cp = new CompilerParameters();
                cp.GenerateExecutable = true;
                cp.OutputAssembly = exeName;
                cp.GenerateInMemory = false;
                cp.TreatWarningsAsErrors = true;
cp.ReferencedAssemblies.Add( "System.dll" );

                CompilerResults cr = provider.CompileAssemblyFromFile(cp,sourceName);
                if (cr.Errors.Count>0)
                {
                    Console.WriteLine("Errors building {0} into {1}",sourceName,cr.PathToAssembly);
                    foreach (CompilerError ce in cr.Errors)
                    {
                        Console.WriteLine(" {0}", ce.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Source {0} \nbuilt  into {1} successfully.",sourceName,cr.PathToAssembly);
                }
                if (cr.Errors.Count>0)
                {
                    compileOK = false;
                }
                else
                {
                    compileOK = true;
                }
            }
            return compileOK;
        }
    }
}

