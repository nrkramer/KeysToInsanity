using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Base
{
    class Level
    {
        public Stage[] stages;
        public int stageWithKey = 0;
        public int stageWithDoor = 0;
        private string levelName = "";
        private Sound levelMusic;

        public Level(string name, int stages)
        {
            this.stages = new Stage[stages];
            levelName = name;
        }

        public void setMusic(Sound music)
        {
            levelMusic = music;
            levelMusic.play(true);
        }

        public void stopMusic()
        {
            if (levelMusic != null)
                levelMusic.stop();
        }

        public void addStage(Stage stage, int index)
        {
            if (stage.key != null)
                stageWithKey = index;

            if (stage.door != null)
                stageWithDoor = index;

            stages[index] = stage;
        }

        public string getLevelName()
        {
            return levelName;
        }
    }
}
