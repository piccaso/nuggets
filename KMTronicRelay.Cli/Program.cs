using System;
using System.Threading;
using System.Threading.Tasks;

namespace KMTronicRelay.Cli
{
    public class Program
    {
        static void Main(string[] args)
        {
            Delays.OffTime = 200;
            Delays.OnTime = 600;

            var cts = new CancellationTokenSource();

            var bg = new Thread(() => Background(cts.Token));
            bg.Start();

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cts.Cancel();
            };

            void WriteMsg()
            {
                if(cts.IsCancellationRequested) return;
                var msg = $"on= rt- {Delays.OnTime,4} +ui " +
                          $"off= fg- {Delays.OffTime,4} +jk";
                Console.WriteLine(msg);
            };
            WriteMsg();

            while (!cts.Token.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    var c = Console.ReadKey(true);
                    switch (c.Key)
                    {
                        case ConsoleKey.R: Delays.OnTime -= 100; break;
                        case ConsoleKey.T: Delays.OnTime -= 10; break;
                        case ConsoleKey.U: Delays.OnTime += 10; break;
                        case ConsoleKey.I: Delays.OnTime += 100; break;

                        case ConsoleKey.F: Delays.OffTime -= 100; break;
                        case ConsoleKey.G: Delays.OffTime -= 10; break;
                        case ConsoleKey.J: Delays.OffTime += 10; break;
                        case ConsoleKey.K: Delays.OffTime += 100; break;

                        case ConsoleKey.Q:
                        case ConsoleKey.X:
                        case ConsoleKey.Escape:
                        case ConsoleKey.End:
                            cts.Cancel();
                            break;
                    }
                    WriteMsg();
                }
                else
                {
                    Sleep(100, cts.Token);
                }
                
            }
            Console.WriteLine("shutting down...");
            bg.Join();

        }

        private static void Sleep(int msec, CancellationToken ct)
        {
            try
            {
                if (msec < 1 || ct.IsCancellationRequested) return;
                if (msec > 10000) msec = 10000;
                Task.Delay(msec, ct).Wait(ct);
            }
            catch (OperationCanceledException)
            {
                // all right i get it
            }
        }

        private static void Background(CancellationToken ct)
        {
            Thread.CurrentThread.IsBackground = true;

            KmtronicU1CrbRelay.Use("COM3", relay =>
            {
                while (!ct.IsCancellationRequested)
                {
                    if (Delays.OnTime > 0)
                    {
                        relay.Switch(SwitchNumber.One, SwitchAction.On);
                        Sleep(Delays.OnTime, ct);
                    }

                    if (Delays.OffTime > 0)
                    {
                        relay.Switch(SwitchNumber.One, SwitchAction.Off);
                        Sleep(Delays.OffTime, ct);
                    }
                }
            });
        }

        public static readonly Delays Delays = new Delays();
    }

    public class Delays
    {
        private int _offTime;
        private int _onTime;
        private readonly object _sync = new object();

        public int OffTime
        {
            get
            {
                lock (_sync)
                {
                    return _offTime;
                }
            }

            set
            {
                lock (_sync)
                {
                    _offTime = value;
                }
            }
        }

        public int OnTime
        {
            get
            {
                lock (_sync)
                {
                    return _onTime;
                }
            }

            set
            {
                lock (_sync)
                {
                    _onTime = value;
                }
            }
        }
    }
}
