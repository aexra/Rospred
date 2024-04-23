using System.Diagnostics;
using System.Text;

namespace Backend.Processor;

public static class IOProcess
{
    /// <summary>
    /// Returns an exitcode of certain process defined by path and arguments as return type and standart output as out argument.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="args"></param>
    /// <param name="output"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static IOProcessResult Run(string cmdpromt)
    {
        var proc = new Process();

        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = "/c" + cmdpromt;
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardInput = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.RedirectStandardError = true;

        proc.Start();

        var output = proc.StandardOutput.ReadToEnd();
        var error = proc.StandardError.ReadToEnd();

        proc.WaitForExit();

        return new() { ExitCode = proc.ExitCode, Output = proc.ExitCode == 0? output : "Subprocess error: \t" + error };
    }

    //private static string UTF8(this string s)
    //{
    //    byte[] bytes = Encoding.Default.GetBytes(s);
    //    return Encoding.UTF8.GetString(bytes);
    //}

    //private static string CP1251(this string s)
    //{
    //    byte[] bytes = Encoding.Default.GetBytes(s);
    //    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    //    var w1251 = Encoding.GetEncoding("windows-1251");
    //    return w1251.GetString(Encoding.Convert(Encoding.UTF8, w1251, bytes));
    //}
}
