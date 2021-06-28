using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HrApp.Controls
{
    public partial class LoadBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Initial(Button button)
        {
            this.Initial(new Button[] { button }, new LinkButton[] { }, new ImageButton[] { });
        }

        public void Initial(Button[] buttons)
        {
            this.Initial(buttons, new LinkButton[] { }, new ImageButton[] { });
        }

        public void Initial(LinkButton link)
        {
            this.Initial(new Button[] { }, new LinkButton[] { link }, new ImageButton[] { });
        }

        public void Initial(LinkButton[] links)
        {
            this.Initial(new Button[] { }, links, new ImageButton[] { });
        }

        public void Initial(ImageButton imagebutton)
        {
            this.Initial(new Button[]{},new LinkButton[] { }, new ImageButton[] { imagebutton });
        }

        public void Initial(ImageButton[] imagebuttons)
        {
            this.Initial(new Button[] { }, new LinkButton[] { }, imagebuttons);
        }

        public void Initial(Button[] buttons, LinkButton[] links, ImageButton[] imagebuttons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].OnClientClick = "show_loadbar();";
            }

            for (int i = 0; i < links.Length; i++)
            {
                links[i].OnClientClick = "show_loadbar();";
            }

            for (int i = 0; i < imagebuttons.Length; i++)
            {
                imagebuttons[i].OnClientClick = "show_loadbar();";
            }
        }
    }
}