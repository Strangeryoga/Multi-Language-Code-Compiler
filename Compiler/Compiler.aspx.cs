using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ClearScript.V8;





namespace Compiler
{
    public partial class Compiler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {



        }

        protected void csharpButton_Click(object sender, EventArgs e)
        {
            // Set the C# code snippet in the codeTextBox
            codeTextBox.Text = @"using System;

                namespace Compiler
                {
                    public class HelloWorld
                    {
                        public static void Main(string[] args)
                        {
                            Console.WriteLine(""Hello, C#!"");
                        }
                    }
                }";
        }

        protected void javaButton_Click(object sender, EventArgs e)
        {
            // Set the Java code snippet in the codeTextBox
            codeTextBox.Text = @"public class HelloWorld {
                public static void main(String[] args) {
                    System.out.println(""Hello, Java!"");
                }
            }";
        }

        protected void javascriptButton_Click(object sender, EventArgs e)
        {
            // Set the JavaScript code snippet in the codeTextBox
            codeTextBox.Text = @"console.log('Hello, JavaScript!');";
        }


        protected void compileButton_Click(object sender, EventArgs e)
        {
            try
            {

                string code = codeTextBox.Text;
                string language = DetectLanguage(code);

                CodeDomProvider codeProvider;
                CompilerParameters compilerParams = new CompilerParameters();
                CompilerResults results;

                if (language == "C#")
                {
                    codeProvider = CodeDomProvider.CreateProvider("CSharp");
                    compilerParams = new CompilerParameters();
                    compilerParams.GenerateExecutable = false;
                    compilerParams.GenerateInMemory = true;
                    results = codeProvider.CompileAssemblyFromSource(compilerParams, codeTextBox.Text);

                    if (results.Errors.Count > 0)
                    {
                        string errors = "";
                        foreach (CompilerError error in results.Errors)
                        {
                            errors += $"Line {error.Line}: {error.ErrorText}<br/>";
                        }
                        resultLabel.Text = $"Compilation failed:<br/>{errors}";
                        resultLabel.ForeColor = System.Drawing.Color.Red;


                    }
                    else
                    {
                        resultLabel.Text = "Compilation successful!<br/>";
                        resultLabel.ForeColor = System.Drawing.Color.Green;



                        StringWriter outputWriter = new StringWriter();
                        Console.SetOut(outputWriter);

                        Type compiledType = results.CompiledAssembly.GetType("Compiler.HelloWorld");
                        if (compiledType != null)
                        {
                            object instance = Activator.CreateInstance(compiledType);
                            MethodInfo mainMethod = compiledType.GetMethod("Main", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(string[]) }, null);
                            if (mainMethod != null)
                            {
                                mainMethod.Invoke(instance, new object[] { new string[] { } });
                            }
                            else
                            {
                                resultLabel.Text += "Main method not found.";
                            }
                        }

                        string output = outputWriter.ToString();
                        resultLabel.Text += $"Output:<br/>{output}";
                    }
                }
                else if (language == "Java")
                {
                    string codeFilePath = Path.Combine(Server.MapPath("~/Temp"), "HelloWorld.java");
                    File.WriteAllText(codeFilePath, code);
                    CompileJava(codeFilePath);
                }
                else if (language == "JavaScript")
                {

                    using (var engine = new V8ScriptEngine())
                    {
                        // Capture standard output
                        var outputWriter = new StringWriter();
                        engine.AddHostObject("console", new ConsoleWrapper(outputWriter));

                        engine.Execute(code);
                        var output = outputWriter.ToString(); // Retrieve standard output

                        resultLabel.Text = $"Output:<br/>{output}";

                    }

                }
                else
                {
                    resultLabel.Text = "Unsupported language.";
                    resultLabel.ForeColor = System.Drawing.Color.Red;
                    return;
                }

            }
            catch (Exception ex)
            {

                resultLabel.Text = $"An error occurred: {ex.Message}";
                resultLabel.ForeColor = System.Drawing.Color.Red;

            }

        }

        // Helper class to capture console output
        public class ConsoleWrapper
        {
            private StringWriter _outputWriter;

            public ConsoleWrapper(StringWriter outputWriter)
            {
                _outputWriter = outputWriter;
            }

            public void log(object obj)
            {
                _outputWriter.WriteLine(obj?.ToString());
            }
        }


        private void CompileJava(string codeFilePath)
        {
            string javaCompilerPath = "javac";
            string arguments = $"\"{codeFilePath}\""; 
            Process process = new Process();
            process.StartInfo.FileName = javaCompilerPath;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (process.ExitCode == 0)
            {
                resultLabel.Text = $"Compilation successful!<br/>";
                resultLabel.ForeColor = System.Drawing.Color.Green;


                // Execute the compiled Java code
                ExecuteJava(codeFilePath.Replace(".java", ".class"));
            }
            else
            {
                resultLabel.Text = $"Compilation failed: {error}";
                resultLabel.ForeColor = System.Drawing.Color.Red;
            }
        }


        private void ExecuteJava(string compiledFilePath)
        {
            string javaInterpreterPath = "java"; 
            string className = Path.GetFileNameWithoutExtension(compiledFilePath);
            string arguments = $"{className}";

            Process process = new Process();
            process.StartInfo.FileName = javaInterpreterPath;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(compiledFilePath);

            process.Start();
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (process.ExitCode == 0)
            {
                resultLabel.Text += $"Output:<br/>{output}";
            }
            else
            {
                resultLabel.Text += $"Execution failed: {error}";
            }
        }


        private string DetectLanguage(string code)
        {
            // For simplicity, let's assume the code starts with specific keywords.
            if (code.TrimStart().StartsWith("using") || code.TrimStart().StartsWith("namespace") || code.TrimStart().StartsWith("class"))
            {
                return "C#";
            }
            else if (code.TrimStart().StartsWith("import") || code.TrimStart().StartsWith("class") || code.TrimStart().StartsWith("public"))
            {
                return "Java";
            }
            else if (code.TrimStart().StartsWith("function") || code.TrimStart().StartsWith("const") || code.TrimStart().StartsWith("let") || code.TrimStart().StartsWith("var") || code.TrimStart().StartsWith("console"))
            {
                return "JavaScript";
            }
            else
            {
                return "Unsupported";
            }
        }

    }
}