using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.IO;

public class mind_script : MonoBehaviour
{
    bool confirmed = false;
    public float selectedFreq = -1;
    public int duration = 1;
    Text msg;
    Text text_linux;
    // for socket
    public int port = 8080;
    IPAddress serverAddress = IPAddress.Parse("127.0.0.1");
    int received = -1;
    byte[] buffer = new byte[2048];
    Socket socket;
    // tree
    QuadTree head;
    QuadTree current;

    // freq options
    float one = 7, two = 8, three = 9, four = 10;

    // Discord bot server
    public string bot_post_url = "https://localhost/send";
    // Linux 
    public string linux_host = "root@fedora";

    void Start()
    {
        msg = GameObject.Find("msg").GetComponent<Text>();
        text_linux = GameObject.Find("Text_Linux").GetComponent<Text>();
        Queue<string> queue = new Queue<string>();
        for (int i = 'a'; i <= 'z'; i++)
        {
            queue.Enqueue(((char)i).ToString());
        }
        for (int i = '0'; i <= '9'; i++)
        {
            queue.Enqueue(((char)i).ToString());
        }
        // 26 + 10 = 36
        queue.Enqueue("Space");
        queue.Enqueue(','.ToString());
        queue.Enqueue('.'.ToString());
        queue.Enqueue('!'.ToString());
        queue.Enqueue('@'.ToString());
        queue.Enqueue('?'.ToString());
        queue.Enqueue('<'.ToString());
        queue.Enqueue('>'.ToString());
        queue.Enqueue('('.ToString());
        queue.Enqueue(')'.ToString());
        queue.Enqueue('|'.ToString());
        queue.Enqueue("Linux");
        queue.Enqueue("Discord");
        queue.Enqueue("Backspace");
        queue.Enqueue("Speek");
        queue.Enqueue("Clear");
        head = new QuadTree();
        int depth = 1;
        QuadTree.Build(head, queue, depth, 3);
        current = head;
        show(current);

        // ʵ����socket
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(new IPEndPoint(serverAddress, port));

        // ����socket����
        Thread thread;
        thread = new Thread(Connection);
        thread.Start(socket);





    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Keypad1) & confirmed == false) StartCoroutine(Confirmed(one));
        if (Input.GetKeyUp(KeyCode.Keypad2) & confirmed == false) StartCoroutine(Confirmed(two));
        if (Input.GetKeyUp(KeyCode.Keypad3) & confirmed == false) StartCoroutine(Confirmed(three));
        if (Input.GetKeyUp(KeyCode.Keypad4) & confirmed == false) StartCoroutine(Confirmed(four));
        // Test
        if (Input.GetKeyUp(KeyCode.Keypad9) & confirmed == false) Discord("test");
        if (received > 0 & confirmed == false) StartCoroutine(Confirmed(received));
    }

    IEnumerator Confirmed(float freq)
    {
        confirmed = true;
        Debug.Log("Freq = " + freq);
        selectedFreq = freq;

        if (freq == one)
        {
            if (current.one.one is null)
            {
                if (!isSpecialFunction(current.one.data)) msg.text += current.one.data;
                current = head;
            }
            else
            {
                current = current.one;
            }
        }
        if (freq == two)
        {
            if (current.two.one is null)
            {
                if (!isSpecialFunction(current.two.data)) msg.text += current.two.data;
                current = head;
            }
            else
            {
                current = current.two;
            }
        }
        if (freq == three)
        {
            if (current.three.one is null)
            {
                if (!isSpecialFunction(current.three.data)) msg.text += current.three.data;
                current = head;
            }
            else
            {
                current = current.three;
            }
        }
        if (freq == four)
        {
            if (current.four.one is null)
            {
                if (!isSpecialFunction(current.four.data)) msg.text += current.four.data;
                current = head;
            }
            else
            {
                current = current.four;
            }
        }
        bool isSpecialFunction(string data)
        {
            switch (data)
            {
                case "Backspace":
                    backspace();    // ɾ��
                    return true;
                case "Linux":
                    Linux(msg.text);    // ִ��Linux����
                    msg.text = "";
                    return true;
                case "Discord":
                    Discord(msg.text);  // ��DiscordƵ������Ϣ
                    msg.text = "";
                    return true;
                case "Space":
                    msg.text += " ";    // �ո�
                    return true;
                case "Speek":
                    Speeker.Speek(msg.text);
                    return true;
                case "Clear":
                    msg.text = "";
                    return true;
                default:
                    return false;
            }
        }

        show(current);
        yield return new WaitForSecondsRealtime(duration);

        confirmed = false;
        selectedFreq = -1;
        received = -1;
        yield return null;
    }

    void Connection(object obj)
    {
        Debug.Log("�����߳������ɹ���");
        while (true)
        {
            Socket socket = obj as Socket;
            try
            {
                int result = socket.Receive(buffer);
                if (result == 0)
                {
                    break;
                }
                else
                {
                    string str = Encoding.Default.GetString(buffer, 0, result);
                    if (str == "0")
                    {
                        received = 7;
                    }
                    if (str == "1")
                    {
                        received = 8;
                    }
                    if (str == "2")
                    {
                        received = 9;
                    }
                    if (str == "3")
                    {
                        received = 10;
                    }
                    Debug.Log("Confirm");
                    
                    Debug.Log($"���յ����ݣ�{str}");

                }
            }
            catch (Exception e)
            {
                Debug.Log("��������" + e.Message);
            }
        }

    }

    void show(QuadTree node)
    {
        var one = new List<string>();
        var two = new List<string>();
        var three = new List<string>();
        var four = new List<string>();

        QuadTree.GetTree(node.one, one);
        QuadTree.GetTree(node.two, two);
        QuadTree.GetTree(node.three, three);
        QuadTree.GetTree(node.four, four);

        GameObject.Find("text_lu").GetComponent<Text>().text = string.Join(" ", one);
        GameObject.Find("text_ru").GetComponent<Text>().text = string.Join(" ", two);
        GameObject.Find("text_lb").GetComponent<Text>().text = string.Join(" ", three);
        GameObject.Find("text_rb").GetComponent<Text>().text = string.Join(" ", four);
    }

    void backspace()
    {
        if (msg.text.Length > 0) msg.text = msg.text.Substring(0, msg.text.Length - 1);
    }
    void Linux(string command)
    {
        var p = new System.Diagnostics.Process();
        p.StartInfo.FileName = "ssh";
        p.StartInfo.Arguments = $"{linux_host} {command}";
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.CreateNoWindow = true;
        p.Start();
        string output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        text_linux.text = output;
    }
    async void Discord(string msg)
    {
        var key = Environment.GetEnvironmentVariable("BCI_KEY");

        var message = new Message(msg, key);

        var json = JsonConvert.SerializeObject(message);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.PostAsync(bot_post_url, data);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //this.msg.text = await response.Content.ReadAsStringAsync();
            }
            else
            {
                this.msg.text = "?";
            }
        }
    }
    class Message
    {
        public string text;
        public string key;
        public Message(string text, string key)
        {
            this.text = text;
            this.key = key;
        }
    }


    static class Speeker
    {
        static public void Speek(string message)
        {

            string err = "Do not include special character �벻Ҫ���������ַ�";
            var p = new System.Diagnostics.Process();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // tts.ps1 
                string content = @"param(
    # Parameter help description
    [Parameter()]
    [string]
    $message
)
Add-Type -AssemblyName System.Speech
$Speech = New-Object System.Speech.Synthesis.SpeechSynthesizer
$Speech.Speak($message)";
                Console.WriteLine(content);

                if (File.Exists("tts.ps1"))
                {
                    Console.WriteLine(Directory.GetCurrentDirectory());
                }
                else
                {
                    // Write file
                    using (StreamWriter sw = new StreamWriter("tts.ps1"))
                    {
                        sw.Write(content);
                    }
                }
                p.StartInfo.FileName = "powershell";
                p.StartInfo.Arguments = $@"powershell -ExecutionPolicy Bypass -File .\tts.ps1 '{message}'";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
            }
            else
            {
                p.StartInfo.FileName = "espeak-ng";
                p.StartInfo.Arguments = $@" -vzh '{message}'";
            }

            p.Start();
            p.WaitForExit();
            if (p.ExitCode == 1)
            {
                p.StartInfo.Arguments = $@"powershell -ExecutionPolicy Bypass -File .\tts.ps1 '{err}'";
                p.Start();
            }
        }
    }
}

//