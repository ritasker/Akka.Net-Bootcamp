namespace WinTail
{
    using Akka.Actor;

    #region Program

    internal class Program
    {
        public static ActorSystem MyActorSystem;

        private static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            var consoleWriterActorProps = Props.Create(() => new ConsoleWriterActor());
            var consoleWriterActor = MyActorSystem.ActorOf(consoleWriterActorProps, "consoleWriterActor");

            var validationActorProps = Props.Create<ValidationActor>(consoleWriterActor);
            var validationActor = MyActorSystem.ActorOf(validationActorProps, "validationActor");

            var consoleReaderActorProps = Props.Create<ConsoleReaderActor>(validationActor);
            var consoleReaderActor = MyActorSystem.ActorOf(consoleReaderActorProps, "consoleReaderActor");

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.AwaitTermination();
        }
    }

    #endregion
}