using System;
using System.Media;

namespace cybersecurity_awareness_chatbot_p2
{//start of namespace
    public class greet_user
    {//start of class
        public greet_user()
        {//start of constructor

            //auto get the path of the voice_recording.wav
            string full_path = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\" ,@"\voice_recording.wav" );

            /*creating an instance of the soundPlayer class 
             * with an object name greet*/
            SoundPlayer greet = new SoundPlayer(full_path);

            //then greet the user
            greet.Play();

        }//end of constructor

    }//end of class

}//end of namespace