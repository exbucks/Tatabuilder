using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace TataBuilder
{
    public partial class TimeLineView : UserControl
    {

        #region Properties

        //========================================================= Measurements ==============================================================//

        private int zoom;
        [Browsable(true), Category("Measurements"), DefaultValue(1), Description("Zoom value")]
        public int Zoom { get { return zoom; } set { zoom = value; this.Refresh(); } }

        private int rowHeight;
        [Browsable(true), Category("Measurements"), DefaultValue(50), Description("Height of row")]
        public int RowHeight { get { return rowHeight; } set { rowHeight = value; this.Refresh(); } }

        private float widthPerTimeScale;
        [Browsable(true), Category("Measurements"), DefaultValue(5), Description("Width for 1s when scale is 1")]
        public float WidthPerTimeScale { get { return widthPerTimeScale; } set { widthPerTimeScale = value; this.Refresh(); } }

        private float guidelineSpacing;
        [Browsable(true), Category("Measurements"), DefaultValue(40), Description("Desired spacing width beetween guidelines")]
        public float GuidelineSpacing { get { return guidelineSpacing; } set { guidelineSpacing = value; this.Refresh(); } }

        //========================================================= Colors ==============================================================//

        private Color borderColor;
        [Browsable(true), Category("Colors"), Description("Border Color")]
        public Color BorderColor { get { return borderColor; } set { borderColor = value; this.Refresh(); } }

        private Color fixedColor;
        [Browsable(true), Category("Colors"), Description("Fixed Color")]
        public Color FixedColor { get { return fixedColor; } set { fixedColor = value; this.Refresh(); } }

        private Color guidelineFixedColor;
        [Browsable(true), Category("Colors"), Description("Guideline Color at fixed bar")]
        public Color GuidelineFixedColor { get { return guidelineFixedColor; } set { guidelineFixedColor = value; this.Refresh(); } }

        private Color guidelineColor;
        [Browsable(true), Category("Colors"), Description("Guideline Color")]
        public Color GuidelineColor { get { return guidelineColor; } set { guidelineColor = value; this.Refresh(); } }

        private Color workgroundColor;
        [Browsable(true), Category("Colors"), Description("Workground Background Color")]
        public Color WorkgroundColor { get { return workgroundColor; } set { workgroundColor = value; this.Refresh(); } }

        private Color rowBackColor;
        [Browsable(true), Category("Colors"), Description("Row Background Color")]
        public Color RowBackColor { get { return rowBackColor; } set { rowBackColor = value; this.Refresh(); } }

        private Color rowBorderColor;
        [Browsable(true), Category("Colors"), Description("Row Border Color")]
        public Color RowBorderColor { get { return rowBorderColor; } set { rowBorderColor = value; this.Refresh(); } }

        private Color rowSettingBackColor;
        [Browsable(true), Category("Colors"), Description("Row Setting Background Color")]
        public Color RowSettingBackColor { get { return rowSettingBackColor; } set { rowSettingBackColor = value; this.Refresh(); } }

        private Color rowSettingBorderColor;
        [Browsable(true), Category("Colors"), Description("Row Setting Border Color")]
        public Color RowSettingBorderColor { get { return rowSettingBorderColor; } set { rowSettingBorderColor = value; this.Refresh(); } }

        private Color rowMarkBarBackColor;
        [Browsable(true), Category("Colors"), Description("Row Mark Bar Background Color")]
        public Color RowMarkBarBackColor { get { return rowMarkBarBackColor; } set { rowMarkBarBackColor = value; this.Refresh(); } }

        private Color rowMarkBarBorderColor;
        [Browsable(true), Category("Colors"), Description("Row Mark Bar Border Color")]
        public Color RowMarkBarBorderColor { get { return rowMarkBarBorderColor; } set { rowMarkBarBorderColor = value; this.Refresh(); } }

        private Color selectedRowBorderColor;
        [Browsable(true), Category("Colors"), Description("Selected Row Border Color")]
        public Color SelectedRowBorderColor { get { return selectedRowBorderColor; } set { selectedRowBorderColor = value; this.Refresh(); } }

        private Color itemSplitColor;
        [Browsable(true), Category("Colors"), Description("Line Color to split items")]
        public Color ItemSplitColor { get { return itemSplitColor; } set { itemSplitColor = value; this.Refresh(); } }

        //========================================================= Other options ==============================================================//

        private Bitmap instantMarkIcon;
        [Browsable(true), Category("Appearance"), Description("Bitmap for instant mark")]
        public Bitmap InstantMarkIcon { get { return instantMarkIcon; } set { instantMarkIcon = value; this.Refresh(); } }

        //========================================================= Runtime Properties ==============================================================//

        private int selectedRowIndex;
        [Browsable(false), DefaultValue(-1), Description("Index of the row was selected currently")]
        public int SelectedRowIndex { get { return selectedRowIndex; } }

        private int selectedItemIndexInRow;
        [Browsable(false), DefaultValue(-1), Description("Index of the item was selected currently in selected row")]
        public int SelectedItemIndexInRow { get { return selectedItemIndexInRow; } }

        private ITimeLineDataSource dataSource;
        [Browsable(false), DefaultValue(null), Description("Data source ")]
        public ITimeLineDataSource DataSource { get { return dataSource; } set { dataSource = value; this.Refresh(); } }

        #endregion

        #region Event Handlers

        public event DrawItemEventHandler DrawRowSetting;
        public event EventHandler RowSettingClick;
        public event LengthChangedEventHandler ItemLengthChanged;
        public event ItemMovedEventHandler ItemMoved;
        public event EventHandler RowSelectionChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler DataChanged;

        #endregion

        #region Constants

        private const int SCROLLBAR_SIZE = 17;          // width of vscrollbar and height of hscrollbar
        private const int TIMEBAR_HEIGHT = 35;          // height of time bar
        private const int ROWSETTING_WIDTH = 35;        // width of rowsetting part in left side
        private const int ROW_MARKBAR_HEIGHT = 15;      // height of markbar for instant item
        private const int INSTANT_MARK_WIDTH = 14;      // width of instant mark
        private const int INSTANT_MARK_HEIGHT = 18;     // height of instant mark
        private const int INSTANT_MARK_OFFSET_Y = 3;    // offset between center y of instant mark and the bottom of row markbar
        private const int INSTANT_ITEM_WIDTH = 30;      // width of instant item
        private const int INSTANT_ITEM_HEIGHT = 30;     // height of instant item

        private const int INSTANT_BOX_HORNSIZE = 4;
        private const int INSTANT_BOX_PADDING = 3;
        private const int INSTANT_BOX_CORNER = 3;

        private const int MIN_MOVING_SIZE = 3;

        #endregion

        #region Internal variables

        private float offsetTime = 0; // starting time in view (unit: s)
        private int offsetTop = 0; // top y coordinates (unit: px)
        private int insertPositionRow = -1;
        private int insertPositionItem = -1;
        private bool itemResizing = false;
        private bool itemMoving = false;
        private Point mouseDownPos;
        private Cursor draggingCursor = null;

        #endregion

        public TimeLineView()
        {
            InitializeComponent();

            // user control settings
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            // properties
            zoom                = 1;
            rowHeight           = 60;
            widthPerTimeScale   = 40;
            guidelineSpacing    = 40;

            borderColor             = Color.FromArgb(190, 190, 190);
            fixedColor              = Color.FromArgb(223, 223, 223);
            guidelineFixedColor     = Color.FromArgb(170, 170, 170);
            guidelineColor          = Color.FromArgb(224, 224, 224);
            workgroundColor         = Color.FromArgb(248, 248, 248);
            rowBackColor            = Color.FromArgb(250, 250, 250);
            rowBorderColor          = Color.FromArgb(190, 190, 190);
            rowSettingBackColor     = Color.FromArgb(207, 207, 207);
            rowSettingBorderColor   = Color.FromArgb(175, 175, 175);
            rowMarkBarBackColor     = Color.FromArgb(222, 255, 255);
            rowMarkBarBorderColor   = Color.FromArgb(224, 224, 224);
            selectedRowBorderColor  = Color.FromArgb(80, 175, 255);
            itemSplitColor          = Color.FromArgb(150, 150, 150);

            instantMarkIcon = null;

            selectedRowIndex = -1;
            selectedItemIndexInRow = -1;
            dataSource = null;

            // init inner controls
            pnlHScrollBarWrapper.Height = SCROLLBAR_SIZE;
            pnlVScrollBarWrapper.Width = SCROLLBAR_SIZE;
            pnlVScrollBarWrapper.Padding = new Padding(0, 0, 0, SCROLLBAR_SIZE);

            // calc the size of scrollbars
            this.CalcScrollbarSize();
        }

        public void NotifyDataSourceChanged()
        {
            // clear selection
            //this.ClearSelection();

            // clear highlight
            this.ClearHighlight();

            // recalc the size of scrollbars
            this.CalcScrollbarSize();

            // redraw
            this.Refresh();

            // call event listener
            if (DataChanged != null)
                DataChanged(this, new EventArgs());
        }

        public void SetSelection(int newSelectedRowIndex, int newSelectedItemIndexInRow)
        {
            bool rowChanged = selectedRowIndex != newSelectedRowIndex;

            selectedRowIndex = newSelectedRowIndex;
            selectedItemIndexInRow = newSelectedItemIndexInRow;
            this.Refresh();

            // call event listener
            if (rowChanged && RowSelectionChanged != null)
                RowSelectionChanged(this, new EventArgs());

            if (SelectionChanged != null)
                SelectionChanged(this, new EventArgs());
        }

        public void ClearSelection()
        {
            bool rowChanged = selectedRowIndex != -1;

            selectedRowIndex = -1;
            selectedItemIndexInRow = -1;

            // call event listener
            if (rowChanged && RowSelectionChanged != null)
                RowSelectionChanged(this, new EventArgs());

            if (SelectionChanged != null)
                SelectionChanged(this, new EventArgs());
        }

        public void ClearHighlight()
        {
            insertPositionRow = -1;
            insertPositionItem = -1;
        }

        public int RowIndexFromPoint(Point pt)
        {
            if (dataSource != null) {
                if (selectedRowIndex != -1 && selectedItemIndexInRow != -1 && dataSource.isInstantItem(selectedRowIndex, selectedItemIndexInRow) && new Rectangle(1, 1, this.ClientRectangle.Width - 2 - SCROLLBAR_SIZE, this.ClientRectangle.Height - 2 - SCROLLBAR_SIZE).Contains(pt)) {
                    int actionCount = dataSource.numberOfItemsInRow(selectedRowIndex);
                    float startTime = 0;
                    int x1, x3, y1, y3;
                    int instantCount = 0;

                    for (int i = 0; i <= selectedItemIndexInRow; i++) {
                        if (dataSource.isInstantItem(selectedRowIndex, i)) {
                            instantCount++;
                        } else {
                            startTime += dataSource.durationOfItem(selectedRowIndex, i);
                            instantCount = 0;
                        }
                    }

                    // x, y position of instant mark base point
                    x1 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime - offsetTime));
                    y1 = TIMEBAR_HEIGHT - offsetTop + selectedRowIndex * rowHeight + ROW_MARKBAR_HEIGHT;

                    for (int i = selectedItemIndexInRow + 1; i < actionCount; i++) {
                        if (dataSource.isInstantItem(selectedRowIndex, i))
                            instantCount++;
                        else
                            break;
                    }

                    // first instant item x, y position of group
                    x3 = x1 - INSTANT_ITEM_WIDTH * instantCount / 2 - 1;
                    y3 = y1 - INSTANT_MARK_HEIGHT - INSTANT_BOX_HORNSIZE - INSTANT_ITEM_HEIGHT - INSTANT_BOX_PADDING + 8;
                    if (new Rectangle(x3, y3, INSTANT_ITEM_WIDTH * actionCount, INSTANT_ITEM_HEIGHT).Contains(pt)) {
                        return selectedRowIndex;
                    }
                }

                if (new Rectangle(1, TIMEBAR_HEIGHT + 1, this.ClientRectangle.Width - 2 - SCROLLBAR_SIZE, this.ClientRectangle.Height - 2 - SCROLLBAR_SIZE).Contains(pt)) {
                    int index = (pt.Y - TIMEBAR_HEIGHT + offsetTop) / rowHeight;
                    if (index >= 0 && index < dataSource.numberOfRows())
                        return index;
                }
            }

            return -1;
        }

        public bool HighlightPositionForInsert(Point pt, out int rowIndex, out int itemIndex)
        {
            if (dataSource != null) {
                // check if point on right side
                if (new Rectangle(ROWSETTING_WIDTH, TIMEBAR_HEIGHT + 1, this.ClientRectangle.Width - 2 - SCROLLBAR_SIZE, this.ClientRectangle.Height - 2 - SCROLLBAR_SIZE).Contains(pt)) {
                    insertPositionRow = (pt.Y - TIMEBAR_HEIGHT + offsetTop) / rowHeight;
                    // check if point on row
                    if (insertPositionRow >= 0 && insertPositionRow < dataSource.numberOfRows()) {
                        // check if point on the out of effect(mark) bar
                        if (pt.Y >= TIMEBAR_HEIGHT - offsetTop + insertPositionRow * rowHeight + ROW_MARKBAR_HEIGHT) {

                            int actionCount = dataSource.numberOfItemsInRow(insertPositionRow);
                            float startTime = 0, duration;
                            int x1, x2;

                            for (insertPositionItem = 0; insertPositionItem < actionCount; insertPositionItem++) {
                                // get duration and x coordinates of item
                                duration = dataSource.durationOfItem(insertPositionRow, insertPositionItem);
                                x1 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime - offsetTime));
                                x2 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime + duration - offsetTime));
                                if (pt.X < (x1 + x2) / 2) 
                                    break;

                                // increate startTime
                                startTime += duration;
                            }

                            // redraw control
                            this.Invalidate();

                            // return success
                            rowIndex = insertPositionRow;
                            itemIndex = insertPositionItem;
                            return true;
                        }
                    }
                }
            }

            // clear insert position
            insertPositionRow = -1;
            insertPositionItem = -1;

            // redraw control
            this.Invalidate();

            rowIndex = 0;
            itemIndex = 0;
            return false;
        }

        private void CalcScrollbarSize()
        {
            if (dataSource != null) {
                int w = this.ClientRectangle.Width;
                int h = this.ClientRectangle.Height;
                int pageWidth = w - SCROLLBAR_SIZE - ROWSETTING_WIDTH;
                int pageHeight = h - SCROLLBAR_SIZE - TIMEBAR_HEIGHT;

                hScrollBar.LargeChange = pageWidth;
                hScrollBar.SmallChange = pageWidth / 10;
                hScrollBar.Maximum = (int)(widthPerTimeScale * zoom * dataSource.totalDuration() + pageWidth / 3);
                hScrollBar.Enabled = hScrollBar.LargeChange < hScrollBar.Maximum;

                vScrollBar.LargeChange = pageHeight;
                vScrollBar.SmallChange = pageHeight / 10;
                vScrollBar.Maximum = (int)(rowHeight * dataSource.numberOfRows()) + 10;
                vScrollBar.Enabled = vScrollBar.LargeChange < vScrollBar.Maximum;
            } else {
                hScrollBar.Enabled = false;
                vScrollBar.Enabled = false;
            }
        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            offsetTime = hScrollBar.Value / (widthPerTimeScale * zoom);
            this.Invalidate();
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            offsetTop = vScrollBar.Value;
            this.Invalidate();
        }

        private void TimeLineView_MouseDown(object sender, MouseEventArgs e)
        {
            itemResizing = false;
            itemMoving = false;

            int oldSelectedRowIndex = selectedRowIndex;
            int oldSelectedItemIndexInRow = selectedItemIndexInRow;

            if (dataSource != null && e.Button == MouseButtons.Left) {
                selectedRowIndex = this.RowIndexFromPoint(e.Location);
                if (selectedRowIndex != -1) {

                    // check if click on instant item
                    if (selectedItemIndexInRow != -1 && dataSource.isInstantItem(selectedRowIndex, selectedItemIndexInRow) && new Rectangle(1, 1, this.ClientRectangle.Width - 2 - SCROLLBAR_SIZE, this.ClientRectangle.Height - 2 - SCROLLBAR_SIZE).Contains(e.Location)) {
                        int actionCount = dataSource.numberOfItemsInRow(selectedRowIndex);
                        float startTime = 0;
                        int x1, x3, y1, y3;
                        int instantCount = 0, startInstant = -1;

                        for (int i = 0; i <= selectedItemIndexInRow; i++) {
                            if (dataSource.isInstantItem(selectedRowIndex, i)) {
                                instantCount++;
                                if (startInstant == -1)
                                    startInstant = i;
                            } else {
                                startTime += dataSource.durationOfItem(selectedRowIndex, i);
                                instantCount = 0;
                                startInstant = -1;
                            }
                        }

                        // x, y position of instant mark base point
                        x1 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime - offsetTime));
                        y1 = TIMEBAR_HEIGHT - offsetTop + selectedRowIndex * rowHeight + ROW_MARKBAR_HEIGHT;

                        for (int i = selectedItemIndexInRow + 1; i < actionCount; i++) {
                            if (dataSource.isInstantItem(selectedRowIndex, i))
                                instantCount++;
                            else
                                break;
                        }

                        // first instant item x, y position of group
                        x3 = x1 - INSTANT_ITEM_WIDTH * instantCount / 2 - 1;
                        y3 = y1 - INSTANT_MARK_HEIGHT - INSTANT_BOX_HORNSIZE - INSTANT_ITEM_HEIGHT - INSTANT_BOX_PADDING + 8;
                        if (new Rectangle(x3, y3, INSTANT_ITEM_WIDTH * actionCount, INSTANT_ITEM_HEIGHT).Contains(e.Location)) {
                            selectedItemIndexInRow = startInstant + (e.X - x3) / INSTANT_ITEM_WIDTH;
                            itemMoving = true;
                            mouseDownPos = new Point(e.X, e.Y);
                            Bitmap icon = dataSource.draggingIconOfItem(selectedRowIndex, selectedItemIndexInRow);
                            draggingCursor = CursorUtil.CreateCursor(icon, icon.Width / 2, icon.Height / 2);
                            this.Invalidate();

                            // call event listener
                            if (selectedRowIndex != oldSelectedRowIndex && RowSelectionChanged != null)
                                RowSelectionChanged(this, new EventArgs());

                            if (SelectionChanged != null && (selectedRowIndex != oldSelectedRowIndex || selectedItemIndexInRow != oldSelectedItemIndexInRow))
                                SelectionChanged(this, new EventArgs());

                            return;
                        }
                    }

                    selectedItemIndexInRow = -1;

                    // check if point on right side
                    if (new Rectangle(ROWSETTING_WIDTH, TIMEBAR_HEIGHT + 1, this.ClientRectangle.Width - 2 - SCROLLBAR_SIZE, this.ClientRectangle.Height - 2 - SCROLLBAR_SIZE).Contains(e.Location)) {
                        // item count of row
                        int actionCount = dataSource.numberOfItemsInRow(selectedRowIndex);

                        // check instant mark
                        {
                            float startTime = 0;
                            int x1;
                            int y1 = TIMEBAR_HEIGHT - offsetTop + selectedRowIndex * rowHeight + ROW_MARKBAR_HEIGHT;
                            for (int i = 0; i < actionCount; i++) {
                                if (dataSource.isInstantItem(selectedRowIndex, i)) {
                                    x1 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime - offsetTime));
                                    if (new Rectangle(x1 - 1 - INSTANT_MARK_WIDTH / 2, y1 - INSTANT_MARK_OFFSET_Y - INSTANT_MARK_HEIGHT / 2, INSTANT_MARK_WIDTH, INSTANT_MARK_HEIGHT).Contains(e.Location)) {
                                        for (int j = i + 1; j < actionCount; j++) {
                                            if (!dataSource.isInstantItem(selectedRowIndex, j))
                                                break;
                                            if (oldSelectedRowIndex == selectedRowIndex && oldSelectedItemIndexInRow == j) {
                                                selectedItemIndexInRow = j;
                                                return;
                                            }
                                        }

                                        selectedItemIndexInRow = i;
                                        this.Invalidate();

                                        // call event listener
                                        if (selectedRowIndex != oldSelectedRowIndex && RowSelectionChanged != null)
                                            RowSelectionChanged(this, new EventArgs());

                                        if (SelectionChanged != null && (selectedRowIndex != oldSelectedRowIndex || selectedItemIndexInRow != oldSelectedItemIndexInRow))
                                            SelectionChanged(this, new EventArgs());

                                        return;
                                    }
                                } else {
                                    startTime += dataSource.durationOfItem(selectedRowIndex, i);
                                }
                            }
                        }

                        // check if point on the out of effect(mark) bar
                        if (e.Y >= TIMEBAR_HEIGHT - offsetTop + selectedRowIndex * rowHeight + ROW_MARKBAR_HEIGHT) {

                            float startTime = 0, duration;
                            int x1, x2;

                            for (int i = 0; i < actionCount; i++) {
                                if (!dataSource.isInstantItem(selectedRowIndex, i)) {
                                    // get duration and x coordinates of item
                                    duration = dataSource.durationOfItem(selectedRowIndex, i);
                                    x1 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime - offsetTime));
                                    x2 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime + duration - offsetTime));
                                    if (e.X >= x2 - 3 && e.X < x2 + 2) {
                                        selectedItemIndexInRow = i;
                                        itemResizing = true;
                                        mouseDownPos = new Point(x1, e.Y);
                                        break;
                                    } else if (e.X >= x1 && e.X < x2) {
                                        selectedItemIndexInRow = i;
                                        itemMoving = true;
                                        mouseDownPos = new Point(e.X, e.Y);
                                        Bitmap icon = dataSource.draggingIconOfItem(selectedRowIndex, selectedItemIndexInRow);
                                        draggingCursor = CursorUtil.CreateCursor(icon, icon.Width / 2, icon.Height / 2);
                                        break;
                                    }

                                    // increate startTime
                                    startTime += duration;
                                }
                            }
                        }
                    }
                }

                this.Invalidate();

                // call event listener
                if (selectedRowIndex != oldSelectedRowIndex && RowSelectionChanged != null)
                    RowSelectionChanged(this, new EventArgs());

                if (SelectionChanged != null && (selectedRowIndex != oldSelectedRowIndex || selectedItemIndexInRow != oldSelectedItemIndexInRow))
                    SelectionChanged(this, new EventArgs());
            }
        }

        private void TimeLineView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None) {
                int rowIndex = this.RowIndexFromPoint(e.Location);
                if (rowIndex != -1) {
                    // check if point on right side
                    if (new Rectangle(ROWSETTING_WIDTH, TIMEBAR_HEIGHT + 1, this.ClientRectangle.Width - 2 - SCROLLBAR_SIZE, this.ClientRectangle.Height - 2 - SCROLLBAR_SIZE).Contains(new Point(e.X, e.Y))) {
                        // item count of row
                        int actionCount = dataSource.numberOfItemsInRow(selectedRowIndex);

                        // check instant mark
                        {
                            float startTime = 0;
                            int x1;
                            int y1 = TIMEBAR_HEIGHT - offsetTop + selectedRowIndex * rowHeight + ROW_MARKBAR_HEIGHT;
                            for (int i = 0; i < actionCount; i++) {
                                if (dataSource.isInstantItem(selectedRowIndex, i)) {
                                    x1 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime - offsetTime));
                                    if (new Rectangle(x1 - 1 - INSTANT_MARK_WIDTH / 2, y1 - INSTANT_MARK_OFFSET_Y - INSTANT_MARK_HEIGHT / 2, INSTANT_MARK_WIDTH, INSTANT_MARK_HEIGHT).Contains(e.Location)) {
                                        this.Cursor = Cursors.Default;
                                        return;
                                    }
                                } else {
                                    startTime += dataSource.durationOfItem(selectedRowIndex, i);
                                }
                            }
                        }

                        // check if point on the out of effect(mark) bar
                        if (e.Y >= TIMEBAR_HEIGHT - offsetTop + rowIndex * rowHeight + ROW_MARKBAR_HEIGHT) {

                            float startTime = 0, duration;
                            int x1, x2;

                            for (int i = 0; i < actionCount; i++) {
                                // get duration and x coordinates of item
                                duration = dataSource.durationOfItem(rowIndex, i);
                                x1 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime - offsetTime));
                                x2 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime + duration - offsetTime));
                                if (e.X >= x2 - 3 && e.X < x2 + 2) {
                                    this.Cursor = Cursors.SizeWE;
                                    return;
                                }

                                // increate startTime
                                startTime += duration;
                            }
                        }
                    }
                }

                this.Cursor = Cursors.Default;

            } else if (e.Button == MouseButtons.Left) {
                // when item is resizing
                if (itemResizing) {
                    if (e.X > mouseDownPos.X) {
                        if (ItemLengthChanged != null) {
                            float length = (e.X - mouseDownPos.X) / (widthPerTimeScale * zoom);
                            ItemLengthChanged(this, new LengthChangedEventArgs(length));
                        }

                        // clear highlight
                        this.ClearHighlight();

                        // recalc the size of scrollbars
                        this.CalcScrollbarSize();

                        // redraw
                        this.Refresh();
                    }
                } else if (itemMoving) {
                    if (Math.Abs(e.X - mouseDownPos.X) >= MIN_MOVING_SIZE || Math.Abs(e.Y - mouseDownPos.Y) >= MIN_MOVING_SIZE) {
                        int rowIndex, itemIndex;
                        HighlightPositionForInsert(e.Location, out rowIndex, out itemIndex);
                        if (draggingCursor != null && this.Cursor != draggingCursor)
                            this.Cursor = draggingCursor;
                    }
                }
            }
        }

        private void TimeLineView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                if (itemMoving) {
                    if (Math.Abs(e.X - mouseDownPos.X) >= MIN_MOVING_SIZE || Math.Abs(e.Y - mouseDownPos.Y) >= MIN_MOVING_SIZE) {
                        int rowIndex, itemIndex;
                        if (HighlightPositionForInsert(e.Location, out rowIndex, out itemIndex) && (selectedRowIndex != rowIndex || (selectedItemIndexInRow != itemIndex && selectedItemIndexInRow != itemIndex - 1))) {
                            if (ItemMoved != null) {
                                if (ItemMoved(this, new ItemMovedEventArgs(selectedRowIndex, selectedItemIndexInRow, rowIndex, itemIndex))) {
                                    if (selectedRowIndex == rowIndex && itemIndex > selectedItemIndexInRow) {
                                        selectedRowIndex = rowIndex;
                                        selectedItemIndexInRow = itemIndex - 1;
                                    } else {
                                        selectedRowIndex = rowIndex;
                                        selectedItemIndexInRow = itemIndex;
                                    }
                                }
                            }

                            // call event listener
                            if (DataChanged != null)
                                DataChanged(this, new EventArgs());
                        }
                    }
                }
            }

            // clear flags
            itemResizing = false;
            itemMoving = false;
            draggingCursor = null;

            this.Cursor = Cursors.Default;

            // clear highlight
            this.ClearHighlight();

            // recalc the size of scrollbars
            this.CalcScrollbarSize();

            // redraw
            this.Refresh();
        }

        private void TimeLineView_MouseClick(object sender, MouseEventArgs e)
        {
            int index = this.RowIndexFromPoint(e.Location);
            if (index != -1) {
                if (RowSettingClick != null) {
                    if (new Rectangle(1, TIMEBAR_HEIGHT, ROWSETTING_WIDTH - 2, this.ClientRectangle.Height - 2 - SCROLLBAR_SIZE).Contains(e.Location)) {
                        RowSettingClick(this, e);
                    }
                }
            }
        }

        private void TimeLineView_Paint(object sender, PaintEventArgs e)
        {
            int w = this.ClientRectangle.Width;
            int h = this.ClientRectangle.Height;
            Graphics g = e.Graphics;

            // clear control background and draw control border
            g.Clear(fixedColor);
            ControlPaint.DrawBorder(g, this.ClientRectangle, borderColor, ButtonBorderStyle.Solid);

            // draw fixed border
            Pen borderPen = new Pen(borderColor);
            g.DrawLine(borderPen, 1, TIMEBAR_HEIGHT - 1, w - 1 - SCROLLBAR_SIZE - 1, TIMEBAR_HEIGHT - 1);
            g.DrawLine(borderPen, ROWSETTING_WIDTH - 1, 1, ROWSETTING_WIDTH - 1, h - 1 - SCROLLBAR_SIZE);

            // draw content area
            Brush contentBrush = new SolidBrush(workgroundColor);
            g.FillRectangle(contentBrush, ROWSETTING_WIDTH, TIMEBAR_HEIGHT, w - 1 - SCROLLBAR_SIZE - ROWSETTING_WIDTH, h - 1 - SCROLLBAR_SIZE - TIMEBAR_HEIGHT);

            // draw time guideline
            float guidelineCountInTimeUnit = (float)Math.Pow(2, Math.Floor(Math.Log(widthPerTimeScale * zoom / guidelineSpacing, 2)));
            float guidelineOffsetWidth = (offsetTime % (1 / guidelineCountInTimeUnit)) * widthPerTimeScale * zoom;
            float guidelineRealWidth = 1 / guidelineCountInTimeUnit * widthPerTimeScale * zoom;
            int guidelineCount = (int)((w - ROWSETTING_WIDTH - SCROLLBAR_SIZE + guidelineOffsetWidth) / guidelineRealWidth);

            Pen guidelineFixedPen = new Pen(guidelineFixedColor);
            Pen guidelinePen = new Pen(guidelineColor);
            Brush timeTickBrush = new SolidBrush(SystemColors.ControlText);
            g.SetClip(new Rectangle(ROWSETTING_WIDTH, 1, w - 1 - SCROLLBAR_SIZE - ROWSETTING_WIDTH, h - 1 - SCROLLBAR_SIZE - 1));
            for (int i = 1; i <= guidelineCount; i++) {
                float x = guidelineRealWidth * i - guidelineOffsetWidth + ROWSETTING_WIDTH - 1;
                g.DrawLine(guidelineFixedPen, x, TIMEBAR_HEIGHT - 4, x, TIMEBAR_HEIGHT - 1);
                g.DrawLine(guidelinePen, x, TIMEBAR_HEIGHT, x, h - 1 - SCROLLBAR_SIZE - 1);

                float t = (offsetTime - offsetTime % (1 / guidelineCountInTimeUnit)) + i * (1 / guidelineCountInTimeUnit);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString(t + "s", this.Font, timeTickBrush, new RectangleF(x - guidelineRealWidth / 2, 0, guidelineRealWidth, TIMEBAR_HEIGHT), sf);
            }

            // draw row
            if (dataSource != null) {
                int n = dataSource.numberOfRows();
                Brush   selectedRowBorderBrush = new SolidBrush(selectedRowBorderColor);
                Pen     itemSplitPen        = new Pen(itemSplitColor);

                // set clip
                g.SetClip(new Rectangle(1, TIMEBAR_HEIGHT, w - 1 - SCROLLBAR_SIZE - 1, h - 1 - SCROLLBAR_SIZE - TIMEBAR_HEIGHT));

                for (int i = 0; i < n; i++) {
                    // save graphics
                    GraphicsState gs = g.Save();

                    // row top
                    int y = TIMEBAR_HEIGHT - offsetTop + i * rowHeight;

                    // row setting part
                    Brush rowSettingBackBrush;
                    Pen rowSettingBorderPen;
                    if (i == selectedRowIndex) {
                        rowSettingBackBrush = new SolidBrush(TUtil.percentColor(rowSettingBackColor, 0.95f));
                        rowSettingBorderPen = new Pen(TUtil.percentColor(rowSettingBorderColor, 0.95f));
                    } else {
                        rowSettingBackBrush = new SolidBrush(rowSettingBackColor);
                        rowSettingBorderPen = new Pen(rowSettingBorderColor);
                    }
                    g.FillRectangle(rowSettingBackBrush, 1, y, ROWSETTING_WIDTH - 2, rowHeight - 1);
                    g.DrawLine(rowSettingBorderPen, 1, y + rowHeight - 1, ROWSETTING_WIDTH - 2, y + rowHeight - 1);

                    if (DrawRowSetting != null) {
                        DrawItemEventArgs args = new DrawItemEventArgs(g, this.Font, new Rectangle(1, y, ROWSETTING_WIDTH - 2, rowHeight - 1), i, DrawItemState.Default, this.ForeColor, this.BackColor);
                        DrawRowSetting(this, args);
                    }

                    // row bar part
                    Brush rowMarkBarBackBrush, rowBackBrush;
                    Pen rowMarkBarBorderPen, rowBorderPen;
                    if (i == selectedRowIndex) {
                        rowMarkBarBackBrush = new SolidBrush(TUtil.percentColor(rowMarkBarBackColor, 0.95f));
                        rowMarkBarBorderPen = new Pen(TUtil.percentColor(rowMarkBarBorderColor, 0.95f));
                        rowBackBrush = new SolidBrush(TUtil.percentColor(rowBackColor, 0.95f));
                        rowBorderPen = new Pen(TUtil.percentColor(rowBorderColor, 0.95f));
                    } else {
                        rowMarkBarBackBrush = new SolidBrush(rowMarkBarBackColor);
                        rowMarkBarBorderPen = new Pen(rowMarkBarBorderColor);
                        rowBackBrush = new SolidBrush(rowBackColor);
                        rowBorderPen = new Pen(rowBorderColor);
                    }
                    g.FillRectangle(rowMarkBarBackBrush, ROWSETTING_WIDTH, y, w - 1 - SCROLLBAR_SIZE - ROWSETTING_WIDTH, ROW_MARKBAR_HEIGHT - 1);
                    g.DrawLine(rowMarkBarBorderPen, ROWSETTING_WIDTH, y + ROW_MARKBAR_HEIGHT - 1, w - 1 - SCROLLBAR_SIZE - 1, y + ROW_MARKBAR_HEIGHT - 1);
                    g.FillRectangle(rowBackBrush, ROWSETTING_WIDTH, y + ROW_MARKBAR_HEIGHT, w - 1 - SCROLLBAR_SIZE - ROWSETTING_WIDTH, rowHeight - ROW_MARKBAR_HEIGHT - 1);
                    g.DrawLine(rowBorderPen, ROWSETTING_WIDTH, y + rowHeight - 1, w - 1 - SCROLLBAR_SIZE - 1, y + rowHeight - 1);

                    // workground clip
                    g.SetClip(new Rectangle(ROWSETTING_WIDTH, 1, w - 1 - SCROLLBAR_SIZE - ROWSETTING_WIDTH, h - 1 - SCROLLBAR_SIZE - 1), CombineMode.Intersect);

                    // action count of row
                    int actionCount = dataSource.numberOfItemsInRow(i);
                    float startTime = 0, duration;
                    int x1 = ROWSETTING_WIDTH, x2 = ROWSETTING_WIDTH;
                    int y1 = y + ROW_MARKBAR_HEIGHT;
                    int y2 = y + rowHeight - 1;
                    int x3 = 0, y3 = 0;
                    
                    // draw interval items
                    for (int j = 0; j < actionCount; j++) {

                        if (!dataSource.isInstantItem(i, j)) {
                            duration = dataSource.durationOfItem(i, j);
                            x1 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime - offsetTime));
                            x2 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime + duration - offsetTime));

                            // if action is in the right out of screen, stop drawing
                            if (x1 > w - 1 - SCROLLBAR_SIZE)
                                break;
                            if (x2 > ROWSETTING_WIDTH && x1 < x2) {
                                // draw item background
                                Rectangle itemRect = new Rectangle(x1, y1, x2 - x1, y2 - y1);
                                Color startingColor, endingColor;
                                if (i == selectedRowIndex && j == selectedItemIndexInRow) {
                                    startingColor = TUtil.percentColor(dataSource.startingColorOfItem(i, j), 0.8f);
                                    endingColor = TUtil.percentColor(dataSource.endingColorOfItem(i, j), 0.8f); ;
                                } else {
                                    startingColor = dataSource.startingColorOfItem(i, j);
                                    endingColor = dataSource.endingColorOfItem(i, j);
                                }

                                LinearGradientBrush itemBrush = new LinearGradientBrush(itemRect, startingColor, endingColor, LinearGradientMode.Vertical);
                                g.FillRectangle(itemBrush, itemRect);
                                g.DrawRectangle(itemSplitPen, x1 - 1, y1 - 1, x2 - x1, y2 - y1 + 1);

                                // backup graphics and set clip
                                GraphicsState itemGS = g.Save();
                                g.SetClip(new Rectangle(x1, y1, x2 - x1 - 1, y2 - y1 - 1), CombineMode.Intersect);

                                // draw item icon
                                Bitmap itemIcon = dataSource.iconOfItem(i, j);
                                if (itemIcon != null) {
                                    g.DrawImage(itemIcon, (x1 + x2) / 2 - itemIcon.Width / 2, (y1 + y2) / 2 - itemIcon.Height / 2);
                                }

                                // draw duration string
                                StringFormat sf = new StringFormat();
                                sf.Alignment = StringAlignment.Far;
                                g.DrawString(duration + "s", this.Font, timeTickBrush, new Rectangle(x1 + 2, y2 - 14, x2 - x1 - 4, 12), sf);

                                g.Restore(itemGS);

                                // highlight for insert
                                if (i == insertPositionRow && j == insertPositionItem) {
                                    Brush highlightBrush = new SolidBrush(Color.FromArgb(60, 0, 0, 0));
                                    g.FillRectangle(highlightBrush, x1 - 3, y1, 5, y2 - y1);
                                }
                            }

                            startTime += duration;
                        }
                    }

                    // draw insert highlight
                    if (i == insertPositionRow && insertPositionItem == actionCount) {
                        Brush highlightBrush = new SolidBrush(Color.FromArgb(60, 0, 0, 0));
                        g.FillRectangle(highlightBrush, x2 - 3, y1, 5, y2 - y1);
                    }

                    // selected row
                    if (i == selectedRowIndex) {
                        g.FillRectangle(selectedRowBorderBrush, ROWSETTING_WIDTH, y - 1, w - 1 - SCROLLBAR_SIZE - ROWSETTING_WIDTH, 2);
                        g.FillRectangle(selectedRowBorderBrush, ROWSETTING_WIDTH, y + rowHeight - 2, w - 1 - SCROLLBAR_SIZE - ROWSETTING_WIDTH, 2);
                    }

                    // draw instant items
                    startTime = 0;
                    bool selectedInstantGroup = false;
                    for (int j = 0; j < actionCount; j++) {

                        if (dataSource.isInstantItem(i, j)) { // Instant Item
                            x1 = (int)(ROWSETTING_WIDTH + widthPerTimeScale * zoom * (startTime - offsetTime));
                            if (x1 >= ROWSETTING_WIDTH - INSTANT_MARK_WIDTH && x1 <= w - 1 - SCROLLBAR_SIZE - INSTANT_MARK_WIDTH) {
                                // check continuous instant item
                                bool continuousInstant = j > 0 && dataSource.isInstantItem(i, j - 1);

                                if (!continuousInstant && instantMarkIcon != null)
                                    g.DrawImage(instantMarkIcon, x1 - 1 - INSTANT_MARK_WIDTH / 2, y1 - INSTANT_MARK_OFFSET_Y - INSTANT_MARK_HEIGHT / 2, INSTANT_MARK_WIDTH, INSTANT_MARK_HEIGHT);

                                if (i == selectedRowIndex) {
                                    // save graphics && set clip, because instant item can be overlaped on timebar
                                    GraphicsState gs2 = g.Save();
                                    g.SetClip(new Rectangle(1, 1, w - 1 - SCROLLBAR_SIZE - ROWSETTING_WIDTH, h - 1 - SCROLLBAR_SIZE - 1));

                                    // continuous instant item
                                    if (continuousInstant) {
                                        if (selectedInstantGroup) {
                                            // calc x position
                                            x3 += INSTANT_ITEM_WIDTH;
                                        }
                                    } else {
                                        int instantCount = 1;
                                        for (int k = j + 1; k < actionCount; k++) {
                                            if (dataSource.isInstantItem(i, k))
                                                instantCount++;
                                            else
                                                break;
                                        }

                                        selectedInstantGroup = selectedItemIndexInRow >= j && selectedItemIndexInRow < j + instantCount;

                                        if (selectedInstantGroup) {
                                            // calc x position
                                            x3 = x1 - INSTANT_ITEM_WIDTH * instantCount / 2 - 1;
                                            y3 = y1 - INSTANT_MARK_HEIGHT - INSTANT_BOX_HORNSIZE - INSTANT_ITEM_HEIGHT - INSTANT_BOX_PADDING + 8;
                                            GraphicsPath path = new GraphicsPath();
                                            path.AddArc(x3 - INSTANT_BOX_PADDING, y3 - INSTANT_BOX_PADDING, INSTANT_BOX_CORNER * 2, INSTANT_BOX_CORNER * 2, 180, 90);
                                            path.AddLine(x3 - INSTANT_BOX_PADDING + INSTANT_BOX_CORNER, y3 - INSTANT_BOX_PADDING, x3 + instantCount * INSTANT_ITEM_WIDTH + INSTANT_BOX_PADDING - INSTANT_BOX_CORNER - 1, y3 - INSTANT_BOX_PADDING);
                                            path.AddArc(x3 + instantCount * INSTANT_ITEM_WIDTH + INSTANT_BOX_PADDING - INSTANT_BOX_CORNER * 2 - 1, y3 - INSTANT_BOX_PADDING, INSTANT_BOX_CORNER * 2, INSTANT_BOX_CORNER * 2, 270, 90);
                                            path.AddLine(x3 + instantCount * INSTANT_ITEM_WIDTH + INSTANT_BOX_PADDING - 1, y3 - INSTANT_BOX_PADDING + INSTANT_BOX_CORNER, x3 + instantCount * INSTANT_ITEM_WIDTH + INSTANT_BOX_PADDING - 1, y3 + INSTANT_ITEM_HEIGHT + INSTANT_BOX_PADDING - INSTANT_BOX_CORNER - 1);
                                            path.AddArc(x3 + instantCount * INSTANT_ITEM_WIDTH + INSTANT_BOX_PADDING - INSTANT_BOX_CORNER * 2 - 1, y3 + INSTANT_ITEM_HEIGHT + INSTANT_BOX_PADDING - INSTANT_BOX_CORNER * 2 - 1, INSTANT_BOX_CORNER * 2, INSTANT_BOX_CORNER * 2, 0, 90);

                                            path.AddLine(x3 + instantCount * INSTANT_ITEM_WIDTH + INSTANT_BOX_PADDING - INSTANT_BOX_CORNER - 1, y3 + INSTANT_ITEM_HEIGHT + INSTANT_BOX_PADDING - 1, x1 + INSTANT_BOX_HORNSIZE - 1, y3 + INSTANT_ITEM_HEIGHT + INSTANT_BOX_PADDING - 1);
                                            path.AddLine(x1 + INSTANT_BOX_HORNSIZE - 1, y3 + INSTANT_ITEM_HEIGHT + INSTANT_BOX_PADDING - 1, x1 - 1, y3 + INSTANT_ITEM_HEIGHT + INSTANT_BOX_PADDING + INSTANT_BOX_HORNSIZE - 1);
                                            path.AddLine(x1 - 1, y3 + INSTANT_ITEM_HEIGHT + INSTANT_BOX_PADDING + INSTANT_BOX_HORNSIZE - 1, x1 - INSTANT_BOX_HORNSIZE - 1, y3 + INSTANT_ITEM_HEIGHT + INSTANT_BOX_PADDING - 1);
                                            path.AddLine(x1 - INSTANT_BOX_HORNSIZE - 1, y3 + INSTANT_ITEM_HEIGHT + INSTANT_BOX_PADDING - 1, x3 - INSTANT_BOX_PADDING + INSTANT_BOX_CORNER, y3 + INSTANT_ITEM_HEIGHT + INSTANT_BOX_PADDING - 1);

                                            path.AddArc(x3 - INSTANT_BOX_PADDING, y3 + INSTANT_ITEM_HEIGHT + INSTANT_BOX_PADDING - INSTANT_BOX_CORNER * 2 - 1, INSTANT_BOX_CORNER * 2, INSTANT_BOX_CORNER * 2, 90, 90);
                                            path.AddLine(x3 - INSTANT_BOX_PADDING, y3 + INSTANT_ITEM_HEIGHT + INSTANT_BOX_PADDING - INSTANT_BOX_CORNER - 1, x3 - INSTANT_BOX_PADDING, y3 - INSTANT_BOX_PADDING + INSTANT_BOX_CORNER);

                                            g.FillPath(rowBackBrush, path);
                                            g.DrawPath(rowBorderPen, path);
                                        }
                                    }

                                    if (selectedInstantGroup) {
                                        // highlight selected item
                                        if (i == selectedRowIndex && j == selectedItemIndexInRow) {
                                            Brush highlightBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0));
                                            g.FillRectangle(highlightBrush, x3, y3, INSTANT_ITEM_WIDTH, INSTANT_ITEM_HEIGHT);
                                        }

                                        // draw item icon
                                        Bitmap itemIcon = dataSource.iconOfItem(i, j);
                                        if (itemIcon != null) {
                                            g.DrawImage(itemIcon, x3 + INSTANT_ITEM_WIDTH / 2 - itemIcon.Width / 2, y3 + INSTANT_ITEM_HEIGHT / 2 - itemIcon.Height / 2);
                                        }
                                    }

                                    // restore graphics state
                                    g.Restore(gs2);
                                }
                            }
                        } else {
                            startTime += dataSource.durationOfItem(i, j);
                        }
                    }

                    // restore graphics
                    g.Restore(gs);
                }
            }
        }
    }

    #region Length Changed Event

    public class LengthChangedEventArgs : EventArgs
    {
        public LengthChangedEventArgs(float length)
        {
            Length = length;
        }

        public float Length { get; set; }
    }

    public delegate void LengthChangedEventHandler(object sender, LengthChangedEventArgs e);

    public class ItemMovedEventArgs : EventArgs
    {
        public ItemMovedEventArgs(int fromRowIndex, int fromItemIndex, int toRowIndex, int toItemIndex)
        {
            FromRowIndex = fromRowIndex;
            FromItemIndex = fromItemIndex;
            ToRowIndex = toRowIndex;
            ToItemIndex = toItemIndex;
        }

        public int FromRowIndex { get; set; }
        public int FromItemIndex { get; set; }
        public int ToRowIndex { get; set; }
        public int ToItemIndex { get; set; }
    }

    public delegate bool ItemMovedEventHandler(object sender, ItemMovedEventArgs e);

    #endregion
}
