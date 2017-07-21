using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyAwesomeCalendar
{
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Calendar))]
    [ToolboxData("<{0}:MyAwesomeCalendar runat=server></{0}:MyAwesomeCalendar>")]
    public class MyAwesomeCalendar : CompositeControl
    {
        GridView gv;
        Button btnBack, btnNext;
        Label lblWeek;
        DropDownList ddlSelection, ddlYears, ddlMonthes;
        List<string> daysOfTheWeek;
        Dictionary<string, int> months;
        int cellInQuestion;
        List<Button> btns;
        List<int> MultipleSelection;
        bool Selection;

        //Color _MouseOverColor = Color.Navy;
        //Color _TextOverColor = Color.White;
        //Color _SelectedRowColor = Color.Red;
        //Color _SelectedRowTextColor = Color.White;

        //#region Properties

        //public Color MouseOverColor
        //{
        //    get { return _MouseOverColor; }
        //    set { _MouseOverColor = value; }
        //}

        //public Color TextOverColor
        //{
        //    get { return _TextOverColor; }
        //    set { _TextOverColor = value; }
        //}

        //public Color SelectedRowColor
        //{
        //    get { return _SelectedRowColor; }
        //    set { _SelectedRowColor = value; }
        //}

        //public Color SelectedRowTextColor
        //{
        //    get { return _SelectedRowTextColor; }
        //    set { _SelectedRowTextColor = value; }
        //}

        //#endregion

        private int PrevSelection
        {
            get { return (int)ViewState["prevSelection"]; }
            set { ViewState["prevSelection"] = value; }
        }

        public List<DateTime> ListSelection
        {
            get { return (List<DateTime>)ViewState["listSelection"]; }
            set { ViewState["listSelection"] = value; }
        }

        private int TodayRow
        {
            get { return (int)ViewState["todayRow"]; }
            set { ViewState["todayRow"] = value; }
        }

        private int TodayCell
        {
            get { return (int)ViewState["todayCell"]; }
            set { ViewState["todayCell"] = value; }
        }

        private bool MonthStateChanged
        {
            get
            {
                EnsureChildControls();
                return false;
            }
            set
            {
                EnsureChildControls();
                if (SelectedMonth == 12)
                {
                    btnNext.Text = new DateTime(SelectedYear, 1, 1).ToString("MMM", CultureInfo.InvariantCulture);
                    btnBack.Text = new DateTime(SelectedYear, SelectedMonth - 1, 1).ToString("MMM", CultureInfo.InvariantCulture);
                }
                else if (SelectedMonth == 1)
                {
                    btnNext.Text = new DateTime(SelectedYear, SelectedMonth + 1, 1).ToString("MMM", CultureInfo.InvariantCulture);
                    btnBack.Text = new DateTime(SelectedYear, 12, 1).ToString("MMM", CultureInfo.InvariantCulture);
                }
                else
                {
                    btnNext.Text = new DateTime(SelectedYear, SelectedMonth + 1, 1).ToString("MMM", CultureInfo.InvariantCulture);
                    btnBack.Text = new DateTime(SelectedYear, SelectedMonth - 1, 1).ToString("MMM", CultureInfo.InvariantCulture);
                }
            }
        }

        private bool DaysStateChanged
        {
            get
            {
                EnsureChildControls();
                return false;
            }
            set
            {
                EnsureChildControls();

                for (int i = 0; i < 7; i++)
                {
                    if (((Label)gv.Rows[2].Cells[i].Controls[0]).Text == FirstDayName)
                    {
                        cellInQuestion = i;
                        break;
                    }
                }

                int counter = 0;
                int newCounter = 1;
                int daysCounter = 0;

                for (int j = 0; j < 42; j++)
                {
                    if (j >= cellInQuestion && daysCounter < DaysInMonth.Count)
                    {
                        btns[j].Text = DaysInMonth[daysCounter].ToString();
                        if (SelectedMonth == DateTime.Today.Month && SelectedYear == DateTime.Today.Year && btns[j].Text == DateTime.Today.Day.ToString())
                        {
                            Color bgColor = Color.Green;
                            gv.Rows[TodayRow].Cells[TodayCell].BackColor = Color.Green;

                            gv.Rows[TodayRow].Cells[TodayCell].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#FF5733','#00FF00')"));
                            gv.Rows[TodayRow].Cells[TodayCell].Attributes.Add("onmouseout", "MouseLeave(this)");
                            //item.Attributes.Add("onmousedown", String.Format("MouseDown(this,'{0}','{1}')", ColorTranslator.ToHtml(this.SelectedRowColor), ColorTranslator.ToHtml(this.SelectedRowTextColor)));
                            gv.Rows[TodayRow].Cells[TodayCell].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                            btns[j].BackColor = bgColor;
                            ((Button)gv.Rows[TodayRow].Cells[TodayCell].Controls[3]).BackColor = bgColor;

                            btns[j].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                            btns[j].Attributes.Add("onmouseout", "MouseLeave(this)");
                            btns[j].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                            ((Button)gv.Rows[TodayRow].Cells[TodayCell].Controls[3]).Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                            ((Button)gv.Rows[TodayRow].Cells[TodayCell].Controls[3]).Attributes.Add("onmouseout", "MouseLeave(this)");
                            ((Button)gv.Rows[TodayRow].Cells[TodayCell].Controls[3]).Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));
                        }
                        else
                        {
                            int[] rowCell = SearchingRowAndCell(j);
                            Color bgColor;
                            if (rowCell[1] == 0 || rowCell[1] == 6)
                            {
                                bgColor = Color.DeepSkyBlue;
                            }
                            else
                            {
                                bgColor = Color.LightGoldenrodYellow;
                            }

                            gv.Rows[rowCell[0]].Cells[rowCell[1]].BackColor = bgColor;

                            gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#FF5733','#00FF00')"));
                            gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("onmouseout", "MouseLeave(this)");
                            //item.Attributes.Add("onmousedown", String.Format("MouseDown(this,'{0}','{1}')", ColorTranslator.ToHtml(this.SelectedRowColor), ColorTranslator.ToHtml(this.SelectedRowTextColor)));
                            gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                            btns[j].BackColor = bgColor;
                            ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).BackColor = bgColor;

                            btns[j].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                            btns[j].Attributes.Add("onmouseout", "MouseLeave(this)");
                            btns[j].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                            ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                            ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("onmouseout", "MouseLeave(this)");
                            ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));
                        }

                        if ((SelectedMonth != DateTime.Today.Month || SelectedYear != DateTime.Today.Year) && gv.Rows[TodayRow].Cells[TodayCell].BackColor == Color.Green)
                        {
                            Color bgColor;
                            if (TodayCell == 0 || TodayCell == 6)
                            {
                                bgColor = Color.DeepSkyBlue;
                                gv.Rows[TodayRow].Cells[TodayCell].BackColor = Color.DeepSkyBlue;
                            }
                            else
                            {
                                bgColor = Color.LightGoldenrodYellow;
                                gv.Rows[TodayRow].Cells[TodayCell].BackColor = Color.LightGoldenrodYellow;
                            }

                            gv.Rows[TodayRow].Cells[TodayCell].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#FF5733','#00FF00')"));
                            gv.Rows[TodayRow].Cells[TodayCell].Attributes.Add("onmouseout", "MouseLeave(this)");
                            //item.Attributes.Add("onmousedown", String.Format("MouseDown(this,'{0}','{1}')", ColorTranslator.ToHtml(this.SelectedRowColor), ColorTranslator.ToHtml(this.SelectedRowTextColor)));
                            gv.Rows[TodayRow].Cells[TodayCell].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));
                        }

                        if (Selection == true)
                        {
                            if (ddlSelection.SelectedIndex == 0)
                            {
                                if (btns[j].Text == SelectedDay.ToString())
                                {
                                    int[] rowCell = SearchingRowAndCell(j);

                                    Color bgColor = Color.Chocolate;
                                    gv.Rows[rowCell[0]].Cells[rowCell[1]].BackColor = bgColor;

                                    gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#FF5733','#00FF00')"));
                                    gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("onmouseout", "MouseLeave(this)");
                                    //item.Attributes.Add("onmousedown", String.Format("MouseDown(this,'{0}','{1}')", ColorTranslator.ToHtml(this.SelectedRowColor), ColorTranslator.ToHtml(this.SelectedRowTextColor)));
                                    gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                                    btns[j].BackColor = bgColor;
                                    ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).BackColor = bgColor;

                                    btns[j].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                                    btns[j].Attributes.Add("onmouseout", "MouseLeave(this)");
                                    btns[j].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                                    ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                                    ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("onmouseout", "MouseLeave(this)");
                                    ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                                    Selection = false;
                                }
                            }
                            else
                            {
                                if (PrevSelection == -1)
                                {
                                    if (btns[j].Text == SelectedDay.ToString())
                                    {
                                        int[] rowCell = SearchingRowAndCell(j);
                                        PrevSelection = j;

                                        Color bgColor = Color.Chocolate;
                                        gv.Rows[rowCell[0]].Cells[rowCell[1]].BackColor = bgColor;

                                        gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#FF5733','#00FF00')"));
                                        gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("onmouseout", "MouseLeave(this)");
                                        //item.Attributes.Add("onmousedown", String.Format("MouseDown(this,'{0}','{1}')", ColorTranslator.ToHtml(this.SelectedRowColor), ColorTranslator.ToHtml(this.SelectedRowTextColor)));
                                        gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                                        btns[j].BackColor = bgColor;
                                        ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).BackColor = bgColor;

                                        btns[j].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                                        btns[j].Attributes.Add("onmouseout", "MouseLeave(this)");
                                        btns[j].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                                        ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                                        ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("onmouseout", "MouseLeave(this)");
                                        ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));
                                    }
                                }
                                else
                                {
                                    ListSelection = new List<DateTime>();
                                    if (btns[j].Text == SelectedDay.ToString())
                                    {
                                        MultipleSelection = new List<int>();
                                        int[] rowCell = SearchingRowAndCell(j);

                                        Color bgColor = Color.Chocolate;
                                        gv.Rows[rowCell[0]].Cells[rowCell[1]].BackColor = bgColor;

                                        gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#FF5733','#00FF00')"));
                                        gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("onmouseout", "MouseLeave(this)");
                                        //item.Attributes.Add("onmousedown", String.Format("MouseDown(this,'{0}','{1}')", ColorTranslator.ToHtml(this.SelectedRowColor), ColorTranslator.ToHtml(this.SelectedRowTextColor)));
                                        gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                                        btns[j].BackColor = bgColor;
                                        ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).BackColor = bgColor;

                                        btns[j].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                                        btns[j].Attributes.Add("onmouseout", "MouseLeave(this)");
                                        btns[j].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                                        ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                                        ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("onmouseout", "MouseLeave(this)");
                                        ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                                        if (j > PrevSelection)
                                        {
                                            for (int m = PrevSelection; m <= j; m++)
                                            {
                                                ListSelection.Add(new DateTime(SelectedYear, SelectedMonth, int.Parse(btns[m].Text)));
                                                MultipleSelection.Add(m);
                                            }
                                        }
                                        else
                                        {
                                            for (int d = j; d <= PrevSelection; d++)
                                            {
                                                ListSelection.Add(new DateTime(SelectedYear, SelectedMonth, int.Parse(btns[d].Text)));
                                                MultipleSelection.Add(d);
                                            }
                                        }

                                        PrevSelection = j;
                                    }
                                }
                            }
                        }

                        daysCounter++;

                        if (daysCounter == DaysInMonth.Count)
                        {
                            if (MultipleSelection != null)
                            {
                                foreach (var item in MultipleSelection)
                                {
                                    Color bgColor;
                                    if (item == PrevSelection)
                                    {
                                        bgColor = Color.Chocolate;
                                    }
                                    else
                                    {
                                        bgColor = Color.RosyBrown;
                                    }

                                    int[] rowCell = SearchingRowAndCell(item);

                                    gv.Rows[rowCell[0]].Cells[rowCell[1]].BackColor = bgColor;

                                    gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#FF5733','#00FF00')"));
                                    gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("onmouseout", "MouseLeave(this)");
                                    //item.Attributes.Add("onmousedown", String.Format("MouseDown(this,'{0}','{1}')", ColorTranslator.ToHtml(this.SelectedRowColor), ColorTranslator.ToHtml(this.SelectedRowTextColor)));
                                    gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                                    ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[0]).BackColor = bgColor;
                                    ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).BackColor = bgColor;

                                    ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[0]).Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                                    ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[0]).Attributes.Add("onmouseout", "MouseLeave(this)");
                                    ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[0]).Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                                    ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                                    ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("onmouseout", "MouseLeave(this)");
                                    ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));
                                }
                            }
                        }
                    }
                    else if (j < cellInQuestion)
                    {
                        btns[j].Text = (PreviousMonthDays - cellInQuestion + 1 + j).ToString();

                        Color bgColor = Color.LightGray;
                        gv.Rows[3].Cells[j].BackColor = Color.LightGray;

                        gv.Rows[3].Cells[j].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#FF5733','#00FF00')"));
                        gv.Rows[3].Cells[j].Attributes.Add("onmouseout", "MouseLeave(this)");
                        //item.Attributes.Add("onmousedown", String.Format("MouseDown(this,'{0}','{1}')", ColorTranslator.ToHtml(this.SelectedRowColor), ColorTranslator.ToHtml(this.SelectedRowTextColor)));
                        gv.Rows[3].Cells[j].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                        btns[j].BackColor = Color.LightGray;
                        ((Button)gv.Rows[3].Cells[j].Controls[3]).BackColor = Color.LightGray;

                        btns[j].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                        btns[j].Attributes.Add("onmouseout", "MouseLeave(this)");
                        btns[j].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                        ((Button)gv.Rows[3].Cells[j].Controls[3]).Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                        ((Button)gv.Rows[3].Cells[j].Controls[3]).Attributes.Add("onmouseout", "MouseLeave(this)");
                        ((Button)gv.Rows[3].Cells[j].Controls[3]).Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                    }
                    else
                    {
                        btns[j].Text = newCounter.ToString();

                        int[] rowCell = SearchingRowAndCell(j);
                        Color bgColor = Color.LightGray;
                        gv.Rows[rowCell[0]].Cells[rowCell[1]].BackColor = Color.LightGray;

                        gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#FF5733','#00FF00')"));
                        gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("onmouseout", "MouseLeave(this)");
                        //item.Attributes.Add("onmousedown", String.Format("MouseDown(this,'{0}','{1}')", ColorTranslator.ToHtml(this.SelectedRowColor), ColorTranslator.ToHtml(this.SelectedRowTextColor)));
                        gv.Rows[rowCell[0]].Cells[rowCell[1]].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                        btns[j].BackColor = Color.LightGray;
                        ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).BackColor = Color.LightGray;

                        btns[j].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                        btns[j].Attributes.Add("onmouseout", "MouseLeave(this)");
                        btns[j].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                        ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                        ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("onmouseout", "MouseLeave(this)");
                        ((Button)gv.Rows[rowCell[0]].Cells[rowCell[1]].Controls[3]).Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                        newCounter++;
                    }
                    counter++;
                }
            }
        }


        private List<int> DaysInMonth
        {
            get { return (List<int>)ViewState["daysInMonth"]; }
            set { ViewState["daysInMonth"] = value; }
        }

        //private int selectedDay;
        private int SelectedDay
        {
            get { return (int)ViewState["selectedDay"]; }
            set { ViewState["selectedDay"] = value; }
        }

        //private int selectedMonth;
        private int SelectedMonth
        {
            get { return (int)ViewState["selectedMonth"]; }
            set
            {
                if (value > 12)
                {
                    value = 1;
                    SelectedYear++;
                }
                else if (value < 1)
                {
                    value = 12;
                    SelectedYear--;
                }

                ViewState["selectedMonth"] = value;

                DaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(SelectedYear, SelectedMonth)).ToList();
                FirstDayName = new DateTime(SelectedYear, SelectedMonth, 1).ToString("ddd", CultureInfo.InvariantCulture);

                if (ViewState["firstTime"] != null)
                {
                    ddlMonthes.SelectedValue = SelectedMonth.ToString();
                    ddlYears.SelectedValue = SelectedYear.ToString();
                }

                if (SelectedMonth == 12)
                {
                    PreviousMonthDays = DateTime.DaysInMonth(SelectedYear, SelectedMonth - 1);
                    NextMonthDays = DateTime.DaysInMonth(SelectedYear, 1);
                }
                else if (SelectedMonth == 1)
                {
                    PreviousMonthDays = DateTime.DaysInMonth(SelectedYear, 12);
                    NextMonthDays = DateTime.DaysInMonth(SelectedYear, SelectedMonth + 1);
                }
                else
                {
                    PreviousMonthDays = DateTime.DaysInMonth(SelectedYear, SelectedMonth - 1);
                    NextMonthDays = DateTime.DaysInMonth(SelectedYear, SelectedMonth + 1);
                }
            }
        }

        //private int selectedYear;
        private int SelectedYear
        {
            get { return (int)ViewState["selectedYear"]; }
            set
            {
                if (value > ListYears.Last())
                {
                    value = ListYears.First();
                }
                else if (value < ListYears.First())
                {
                    value = ListYears.Last();
                }
                ViewState["selectedYear"] = value;

                if (ViewState["firstTime"] != null)
                {
                    DaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(SelectedYear, SelectedMonth)).ToList();
                    FirstDayName = new DateTime(SelectedYear, SelectedMonth, 1).ToString("ddd", CultureInfo.InvariantCulture);
                }
            }
        }

        private int PreviousMonthDays
        {
            get { return (int)ViewState["previousMonthDays"]; }
            set { ViewState["previousMonthDays"] = value; }
        }

        private int NextMonthDays
        {
            get { return (int)ViewState["nextMonthDays"]; }
            set { ViewState["nextMonthDays"] = value; }
        }

        private string FirstDayName
        {
            get { return (string)ViewState["firstDayName"]; }
            set { ViewState["firstDayName"] = value; }
        }

        private List<int> ListYears
        {
            get { return (List<int>)ViewState["listYears"]; }
            set { ViewState["listYears"] = value; }
        }

        //private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return (DateTime)ViewState["selectedDate"]; }
            set { ViewState["selectedDate"] = value; }
        }


        protected override void CreateChildControls()
        {
            Controls.Clear();

            btns = new List<Button>();

            for (int i = 0; i < 42; i++)
            {
                btns.Add(new Button() { ID = "btn_" + i, Width = Unit.Percentage(90), BorderStyle = BorderStyle.None });
            }

            btnBack = new Button() { ID = "btnBack_0_0", Width = Unit.Percentage(100), BackColor = Color.Transparent, BorderStyle = BorderStyle.None };
            btnNext = new Button() { ID = "btnNext_0_2", Width = Unit.Percentage(100), BackColor = Color.Transparent, BorderStyle = BorderStyle.None };

            gv = new GridView() { ID = "gv", ShowHeader = false };
            gv.RowCreated += Gv_RowCreated;

            if (ViewState["firstTime"] == null)
            {
                Selection = false;

                SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

                ListYears = Enumerable.Range(-100, 201).Select(i => new DateTime(DateTime.Today.Year + i, 1, 1).Year).ToList();

                SelectedDay = DateTime.Today.Day;
                SelectedYear = DateTime.Today.Year;
                SelectedMonth = DateTime.Today.Month;

                List<string> monthsString = Enumerable.Range(1, 12).Select(i => new DateTime(DateTime.Today.Year, i, 1).ToString("MMMM", CultureInfo.InvariantCulture)).ToList();
                List<int> monthsInt = Enumerable.Range(1, 12).Select(i => new DateTime(DateTime.Today.Year, i, 1).Month).ToList();

                daysOfTheWeek = Enumerable.Range(1, 7).Select(i => new DateTime(DateTime.Today.Year, 1, i).ToString("ddd", CultureInfo.InvariantCulture)).ToList();
                months = new Dictionary<string, int>();
                for (int i = 0; i < 12; i++)
                {
                    months.Add(monthsString.ToList()[i], monthsInt.ToList()[i]);
                }

                ViewState["daysOfTheWeek"] = daysOfTheWeek;
                ViewState["months"] = months;
            }
            else
            {
                daysOfTheWeek = (List<string>)ViewState["daysOfTheWeek"];
                months = (Dictionary<string, int>)ViewState["months"];
            }

            ControlsCreation();

            if (ViewState["firstTime"] == null)
            {
                MonthStateChanged = true;
                DaysStateChanged = true;

                ViewState["firstTime"] = 0;
            }

            Controls.Add(gv);
        }

        private void ControlsCreation()
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < 9; i++)
            {
                DataRow row = dt.NewRow();
                dt.Rows.Add(row);
            }

            for (int j = 0; j < 7; j++)
            {
                DataColumn clmn = new DataColumn();
                dt.Columns.Add(clmn);
            }

            gv.DataSource = dt;
            gv.DataBind();

            int counter = 0;
            int newCounter = 1;
            int btnsCounter = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            gv.Rows[i].Cells[j].ColumnSpan += 3;
                            btnBack.Click += BtnBack_Click;
                            gv.Rows[i].Cells[j].Controls.Add(btnBack);
                        }
                        else if (j == 1)
                        {
                            ddlMonthes = new DropDownList() { ID = "ddlMonthes_" + i + " " + j, AutoPostBack = true, Width = Unit.Percentage(50) };
                            ddlYears = new DropDownList() { ID = "ddlYears_" + i + " " + j, AutoPostBack = true, Width = Unit.Percentage(50) };

                            ddlMonthes.DataSource = months;
                            ddlMonthes.DataTextField = "Key";
                            ddlMonthes.DataValueField = "Value";
                            ddlMonthes.DataBind();

                            ddlYears.DataSource = ListYears;
                            ddlYears.DataBind();

                            ddlMonthes.SelectedValue = SelectedMonth.ToString();
                            ddlYears.SelectedValue = SelectedYear.ToString();

                            ddlMonthes.SelectedIndexChanged += DdlMonthes_SelectedIndexChanged;
                            ddlYears.SelectedIndexChanged += DdlYears_SelectedIndexChanged;

                            gv.Rows[i].Cells[j].Controls.Add(ddlMonthes);
                            gv.Rows[i].Cells[j].Controls.Add(ddlYears);
                        }
                        else if (j == 2)
                        {
                            gv.Rows[i].Cells[j].ColumnSpan += 3;
                            btnNext.Click += BtnNext_Click;
                            gv.Rows[i].Cells[j].Controls.Add(btnNext);
                        }
                        else
                        {
                            gv.Rows[i].Cells[j].Visible = false;
                        }
                    }
                    else if (i == 1)
                    {
                        if (j == 0)
                        {
                            gv.Rows[i].Cells[j].ColumnSpan += 7;
                            ddlSelection = new DropDownList() { ID = "ddlSelection_" + i + " " + j, Width = Unit.Percentage(20), AutoPostBack = true };
                            ddlSelection.Items.Add("One day selection");
                            ddlSelection.Items.Add("Multiple days selection");

                            ddlSelection.SelectedIndexChanged += DdlSelection_SelectedIndexChanged;

                            gv.Rows[i].Cells[j].Controls.Add(ddlSelection);
                        }
                        else
                        {
                            gv.Rows[i].Cells[j].Visible = false;
                        }
                    }
                    else if (i == 2)
                    {
                        lblWeek = new Label() { ID = "lblWeek_" + i + "_" + j, Text = daysOfTheWeek.ToList()[j] };
                        gv.Rows[i].Cells[j].Controls.Add(lblWeek);
                        if (lblWeek.Text == FirstDayName)
                        {
                            cellInQuestion = j;
                        }
                    }
                    else
                    {
                        if ((j >= cellInQuestion || i > 3) && counter < DaysInMonth.Count)
                        {
                            btns[btnsCounter].Text = DaysInMonth[counter].ToString();
                            btns[btnsCounter].Click += BtnDay_Click;

                            if (SelectedMonth == DateTime.Today.Month && SelectedYear == DateTime.Today.Year)
                            {
                                if (btns[btnsCounter].Text == DateTime.Today.Day.ToString())
                                {
                                    TodayRow = i;
                                    TodayCell = j;
                                    gv.Rows[i].Cells[j].BackColor = Color.Green;
                                }
                            }
                            counter++;
                        }
                        else if (j < cellInQuestion && i == 3)
                        {
                            btns[btnsCounter].Text = (PreviousMonthDays - cellInQuestion + 1 + j).ToString();
                            btns[btnsCounter].Click += BtnDayPrevMonth_Click;
                        }
                        else
                        {
                            btns[btnsCounter].Text = newCounter.ToString();
                            btns[btnsCounter].Click += BtnDayNextMonth_Click;
                            newCounter++;
                        }
                        //ColorConverter cc = new ColorConverter();
                        //Color bgColor = (Color)cc.ConvertFromString("#FF5733");

                        Color bgColor;
                        Button btnSave;
                        if (j > 0 && j < 6)
                        {
                            btns[btnsCounter].BackColor = Color.LightGoldenrodYellow;
                            btnSave = new Button() { ID = "btnSave_" + i + "_" + j, Text = "Save", Width = Unit.Percentage(90), BackColor = Color.LightGoldenrodYellow, BorderStyle = BorderStyle.None };
                            bgColor = Color.LightGoldenrodYellow;
                        }
                        else
                        {
                            btns[btnsCounter].BackColor = Color.DeepSkyBlue;
                            btnSave = new Button() { ID = "btnSave_" + i + "_" + j, Text = "Save", Width = Unit.Percentage(90), BackColor = Color.DeepSkyBlue, BorderStyle = BorderStyle.None };
                            bgColor = Color.DeepSkyBlue;
                        }

                        btns[btnsCounter].Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                        btns[btnsCounter].Attributes.Add("onmouseout", "MouseLeave(this)");
                        btns[btnsCounter].Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));
                        gv.Rows[i].Cells[j].Controls.Add(btns[btnsCounter]);

                        TextBox tbx1 = new TextBox() { ID = "tbx1_" + i + "_" + j, Width = Unit.Percentage(80) };
                        tbx1.Attributes.Add("placeholder", "Working hours");
                        TextBox tbx2 = new TextBox() { ID = "tbx2_" + i + "_" + j, Width = Unit.Percentage(80) };
                        tbx2.Attributes.Add("placeholder", "Hourly rate");

                        btnSave.Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#4E1509','White')"));
                        btnSave.Attributes.Add("onmouseout", "MouseLeave(this)");
                        btnSave.Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));

                        gv.Rows[i].Cells[j].Controls.Add(tbx1);
                        gv.Rows[i].Cells[j].Controls.Add(tbx2);
                        gv.Rows[i].Cells[j].Controls.Add(btnSave);

                        btnsCounter++;
                    }
                }
                gv.Rows[i].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        private void DdlSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            Selection = false;
            PrevSelection = -1;
            MonthStateChanged = true;
            DaysStateChanged = true;
        }

        private void BtnDayNextMonth_Click(object sender, EventArgs e)
        {
            SelectedMonth++;
            SelectedDay = int.Parse(((Button)sender).Text);

            Selection = false;
            PrevSelection = -1;
            SelectedDate = new DateTime(SelectedYear, SelectedMonth, SelectedDay);

            MonthStateChanged = true;
            DaysStateChanged = true;
        }

        private void BtnDayPrevMonth_Click(object sender, EventArgs e)
        {
            SelectedMonth--;
            SelectedDay = int.Parse(((Button)sender).Text);

            Selection = false;
            PrevSelection = -1;
            SelectedDate = new DateTime(SelectedYear, SelectedMonth, SelectedDay);

            MonthStateChanged = true;
            DaysStateChanged = true;
        }

        private void BtnDay_Click(object sender, EventArgs e)
        {
            SelectedDay = int.Parse(((Button)sender).Text);

            Selection = true;
            SelectedDate = new DateTime(SelectedYear, SelectedMonth, SelectedDay);

            MonthStateChanged = true;
            DaysStateChanged = true;
        }

        private void Gv_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex == 0)
            {
                //GridViewRow row = e.Row;
                Color bgColor = Color.Empty;
                e.Row.BackColor = Color.GreenYellow;

                foreach (TableCell item in e.Row.Cells)
                {
                    if (e.Row.Cells.GetCellIndex(item) == 0 || e.Row.Cells.GetCellIndex(item) == 2)
                    {
                        item.Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#FF5733','#FFFFFF')"));
                        item.Attributes.Add("onmouseout", "MouseLeave(this)");
                        //item.Attributes.Add("onmousedown", String.Format("MouseDown(this,'{0}','{1}')", ColorTranslator.ToHtml(this.SelectedRowColor), ColorTranslator.ToHtml(this.SelectedRowTextColor)));
                        item.Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));
                    }
                }
            }
            else if (e.Row.RowIndex == 2)
            {
                foreach (TableCell item in e.Row.Cells)
                {
                    if (e.Row.Cells.GetCellIndex(item) == 0 || e.Row.Cells.GetCellIndex(item) == 6)
                    {
                        item.BackColor = Color.DeepSkyBlue;
                    }
                    else
                    {
                        item.BackColor = Color.LightGoldenrodYellow;
                    }
                }
            }
            else if (e.Row.RowIndex >= 3)
            {
                //GridViewRow row = e.Row;

                foreach (TableCell item in e.Row.Cells)
                {
                    if (e.Row.Cells.GetCellIndex(item) == 0 || e.Row.Cells.GetCellIndex(item) == 6)
                    {
                        Color bgColor = Color.DeepSkyBlue;
                        item.BackColor = Color.DeepSkyBlue;

                        item.Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#FF5733','#00FF00')"));
                        item.Attributes.Add("onmouseout", "MouseLeave(this)");
                        //item.Attributes.Add("onmousedown", String.Format("MouseDown(this,'{0}','{1}')", ColorTranslator.ToHtml(this.SelectedRowColor), ColorTranslator.ToHtml(this.SelectedRowTextColor)));
                        item.Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));
                    }
                    else
                    {
                        Color bgColor = Color.LightGoldenrodYellow;
                        item.BackColor = Color.LightGoldenrodYellow;

                        item.Attributes.Add("onmouseover", String.Format("MouseEnter(this,'#FF5733','#00FF00')"));
                        item.Attributes.Add("onmouseout", "MouseLeave(this)");
                        //item.Attributes.Add("onmousedown", String.Format("MouseDown(this,'{0}','{1}')", ColorTranslator.ToHtml(this.SelectedRowColor), ColorTranslator.ToHtml(this.SelectedRowTextColor)));
                        item.Attributes.Add("OriginalColor", ColorTranslator.ToHtml(bgColor));
                    }
                }
            }
        }

        private void DdlYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            Selection = false;
            PrevSelection = -1;
            SelectedYear = int.Parse(ddlYears.SelectedValue);
            MonthStateChanged = true;
            DaysStateChanged = true;
        }

        private void DdlMonthes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Selection = false;
            PrevSelection = -1;
            SelectedMonth = int.Parse(ddlMonthes.SelectedValue);
            MonthStateChanged = true;
            DaysStateChanged = true;
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            SelectedMonth++;

            Selection = false;
            PrevSelection = -1;
            ddlMonthes.SelectedValue = SelectedMonth.ToString();
            ddlYears.SelectedValue = SelectedYear.ToString();

            MonthStateChanged = true;
            DaysStateChanged = true;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            SelectedMonth--;

            Selection = false;
            PrevSelection = -1;
            ddlMonthes.SelectedValue = SelectedMonth.ToString();
            ddlYears.SelectedValue = SelectedYear.ToString();

            MonthStateChanged = true;
            DaysStateChanged = true;
        }

        private int[] SearchingRowAndCell(int a)
        {
            int[] result = new int[2];

            if (a <= 6)
            {
                result[0] = 3;
                result[1] = a;
            }
            else if (a <= 13)
            {
                result[0] = 4;
                result[1] = a - 7;
            }
            else if (a <= 20)
            {
                result[0] = 5;
                result[1] = a - 14;
            }
            else if (a <= 27)
            {
                result[0] = 6;
                result[1] = a - 21;
            }
            else if (a <= 34)
            {
                result[0] = 7;
                result[1] = a - 28;
            }
            else if (a <= 41)
            {
                result[0] = 8;
                result[1] = a - 35;
            }

            return result;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //string resourceName = "MyCalendarScript.js";

            //ClientScriptManager cs = this.Page.ClientScript;
            //cs.RegisterClientScriptResource(typeof(MyAwesomeCalendar), resourceName);

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("MCSkey"))
            {
                Page.ClientScript.RegisterClientScriptInclude("MCSkey", "MyCalendarScript.js");
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            gv.RenderControl(writer);
        }
    }
}