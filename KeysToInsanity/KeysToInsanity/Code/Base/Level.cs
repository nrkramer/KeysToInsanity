using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Base
{
    class Level
    {
        private Stage[] stages;
        private string levelName = "";

        public Level(string name, int stages)
        {
            this.stages = new Stage[stages];
            levelName = name;
        }

        public void setStage(Stage stage, int index)
        {
            stages[index] = stage;
        }
    }
}
