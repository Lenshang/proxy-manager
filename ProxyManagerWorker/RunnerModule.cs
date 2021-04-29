using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

/// <summary>
/// 运行外部程序类
/// </summary>
public class RunnerModule
{
    private Process process { get; set; }
    private ProcessStartInfo processInfo { get; set; }
    private List<string> message { get; set; }
    private List<string> errorMessage { get; set; }
    public ProcessState state { get; set; }
    public Action OnProcessExit { get; set; }
    public Action<string> OnMessageReceive { get; set; }
    public StreamWriter StandardInput { get; set; }

    public RunnerModule(string program, string arguments,bool enableInput=false)
    {
        this.message = new List<string>();
        this.errorMessage = new List<string>();
        this.processInfo = new ProcessStartInfo(program, arguments);
        //processInfo.Arguments = arguments;
        processInfo.CreateNoWindow = true;   //不创建窗口
        processInfo.UseShellExecute = false;//不使用系统外壳程序启动,重定向输出的话必须设为false
        processInfo.RedirectStandardOutput = true; //重定向输出，而不是默认的显示在dos控制台上
        processInfo.RedirectStandardError = true;
        processInfo.WorkingDirectory = Environment.CurrentDirectory;
        if (enableInput)
        {
            processInfo.RedirectStandardInput = true;
        }
        state = ProcessState.INIT;
    }
    /// <summary>
    /// 启动程序
    /// </summary>
    public void Run()
    {
        state = ProcessState.RUNNING;
        this.process = Process.Start(processInfo);
        process.OutputDataReceived += Process_OutputDataReceived;
        process.ErrorDataReceived += Process_ErrorDataReceived;
        process.Exited += Process_Exited;
        process.EnableRaisingEvents = true;
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        if (processInfo.RedirectStandardInput)
        {
            this.StandardInput = process.StandardInput;
            this.StandardInput.WriteLine();
        }
    }

    /// <summary>
    /// 启动程序并等待结束
    /// </summary>
    public void RunAndWait(int timeout=1000*15)
    {
        Run();
        this.process.WaitForExit(timeout);
    }
    /// <summary>
    /// 启动程序并等待程序结束并返回打印信息
    /// </summary>
    /// <returns></returns>
    public string RunAndWaitReturn(int timeout = 1000 * 15)
    {
        RunAndWait(timeout);
        return GetMessage();
    }
    /// <summary>
    /// 杀死一个程序
    /// </summary>
    public void Kill()
    {
        this.process.Kill();
        this.process.WaitForExit();
    }
    /// <summary>
    /// 获得打印信息
    /// </summary>
    /// <returns></returns>
    public string GetMessage()
    {
        return string.Join("\n", this.message);
    }
    public List<string> GetList()
    {
        return this.message;
    }

    public void SendCmd(string cmd)
    {
        if (this.StandardInput != null)
        {
            this.StandardInput.WriteLine(cmd);
            this.StandardInput.WriteLine();
        }
        else
        {
            throw new Exception("没有启用重定向输入");
        }
    }
    private void Process_Exited(object sender, EventArgs e)
    {
        state = ProcessState.STOP;
        OnProcessExit?.Invoke();
    }

    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        this.message.Add(e.Data);
        OnMessageReceive?.Invoke(e.Data);
    }

    private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        this.message.Add(e.Data);
        errorMessage.Add(e.Data);
        OnMessageReceive?.Invoke(e.Data);
    }

    /// <summary>
    /// 运行一个程序并且等待程序结束获得打印数据
    /// </summary>
    /// <param name="program"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public static string RunProcessAndGetMessage(string program, string arguments,int timeout=1000*15)
    {
        try
        {
            RunnerModule p = new RunnerModule(program, arguments);
            return p.RunAndWaitReturn(timeout);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    /// <summary>
    /// 运行一个程序并且等待程序结束获得打印数据
    /// </summary>
    /// <param name="program"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public static string RunProcessAndGetMessage(string program,int timeout=1000*15)
    {
        if(program.Contains(" ")){
            string _p = program.Substring(0, program.IndexOf(" "));
            string _a = program.Substring(program.IndexOf(" ") + 1);
            return RunProcessAndGetMessage(_p, _a,timeout);
        }
        else
        {
            return RunProcessAndGetMessage(program, "",timeout);
        }
    }
    /// <summary>
    /// 运行一个程序并等待程序结束获得数据
    /// </summary>
    /// <param name="program"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public static List<string> RunProcessAndGetList(string program, string arguments)
    {
        try
        {
            RunnerModule p = new RunnerModule(program, arguments);
            p.RunAndWait();
            return p.GetList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
public enum ProcessState
{
    INIT,
    RUNNING,
    STOP,
    KILLED
}
