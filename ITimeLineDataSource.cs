using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TataBuilder
{
    public interface ITimeLineDataSource
    {
        int numberOfRows();

        int numberOfItemsInRow(int rowIndex);

        float totalDuration();

        bool isInstantItem(int rowIndex, int itemIndex);

        float durationOfItem(int rowIndex, int itemIndex);

        Color startingColorOfItem(int rowIndex, int itemIndex);

        Color endingColorOfItem(int rowIndex, int itemIndex);

        Bitmap iconOfItem(int rowIndex, int itemIndex);

        Bitmap draggingIconOfItem(int rowIndex, int itemIndex);
    }
}
