using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompanyStructure
{
    public partial class Form2 : Form
    {
        // in this form when we move item then we are just moving the item not the child items
        public Form2()
        {
            InitializeComponent();
        }

        private string _movedItem;
        private string _movedItemParent;
        private string _newParent;
        List<Comment> categories;
        List<Comment> glstChildren;
        public string MovedItem { get => _movedItem; set => _movedItem = value; }
        public string MovedItemParent { get => _movedItemParent; set => _movedItemParent = value; }
        public string NewParent { get => _newParent; set => _newParent = value; }
        public List<Comment> GlstChildren { get => glstChildren; set => glstChildren = value; }

        public void SeedData()
        {
            categories = new List<Comment>()
                                          {
                                              new Comment () { Id = 1, Text = "Mark", ParentId = 0},
                                              new Comment() { Id = 2, Text = "Sarah Donald", ParentId = 1 },
                                              new Comment() { Id = 3, Text = "Cassandra Reynolds", ParentId = 2 },
                                              new Comment() { Id = 4, Text = "Tyler Simpson", ParentId = 1 },
                                              new Comment() { Id = 5, Text = "Mary Blue", ParentId = 3 },
                                              new Comment() { Id = 6, Text = "Harry Tobs", ParentId = 4 },
                                              new Comment() { Id = 7, Text = "Bob Sagat", ParentId = 3 },
                                              new Comment() { Id = 8, Text = "Tina Teff", ParentId = 7 },
                                              new Comment() { Id = 9, Text = "Will Turner", ParentId = 8 },
                                              new Comment() { Id = 10, Text = "Thomas Brown", ParentId = 6 },
                                              new Comment() { Id = 11, Text = "George Carrey", ParentId = 4 },
                                              new Comment() { Id = 12, Text = "Gary Styles", ParentId = 4 },
                                              new Comment() { Id = 13, Text = "Bruce Willis", ParentId = 1 },
                                              new Comment() { Id = 14, Text = "Georgina Flangy", ParentId = 1 },
                                              new Comment() { Id = 15, Text = "Sophie Turner", ParentId = 14 },

                                          };
        }
        public void CreateOrganization()
        {
            List<Comment> hierarchy = new List<Comment>();
            hierarchy = categories
                            .Where(c => c.ParentId == 0)
                            .Select(c => new Comment()
                            {
                                Id = c.Id,
                                Text = c.Text,
                                ParentId = c.ParentId,
                               // hierarchy = c.Id.ToString(),
                                Children = GetChildren(categories, c.Id)
                            })
                            .ToList();

            HieararchyWalk(hierarchy, null, treeView1, true);
        }
        public static void HieararchyWalk(List<Comment> hierarchy, TreeNode rootNode, TreeView treeView, bool flag)
        {
            string result = string.Empty;

            if (hierarchy != null)
            {
                foreach (var item in hierarchy)
                {
                    //TreeNode newTreeNode = treeNode.Nodes.Add(item.Id + " " + item.Text + " Hierarchy-" + item.hierarchy);
                    TreeNode newTreeNode = new TreeNode();
                    if (flag && rootNode == null)
                    {
                        newTreeNode = treeView.Nodes.Add(item.Text);
                    }
                    else
                    {
                        newTreeNode = rootNode.Nodes.Add("-" + item.Text);
                    }
                    if (item.Children.Count != 0)
                    {
                        HieararchyWalk(item.Children, newTreeNode, null, false);
                    }
                }
            }
        }

        public static List<Comment> GetChildren(List<Comment> comments, int parentId)
        {
            return comments
                    .Where(c => c.ParentId == parentId)
                    .Select(c => new Comment
                    {
                        Id = c.Id,
                        Text = c.Text,
                        ParentId = c.ParentId,
                       // hierarchy = GetHiera(comments, c, parentId),
                        Children = GetChildren(comments, c.Id)
                    })
                    .ToList();
        }
        //public static string GetHiera(List<Comment> comments, Comment comment, int parentId)
        //{
        //    string hierarchy = comment.Id.ToString();
        //    Comment parentComm = comments.Where(a => a.Id == parentId).FirstOrDefault();

        //    if (parentComm.ParentId != 0)
        //    {
        //        hierarchy = GetHiera(comments, parentComm, parentComm.ParentId) + hierarchy;
        //    }
        //    else
        //    {
        //        hierarchy = parentId.ToString();
        //    }
        //    return hierarchy;
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            var validateMove = categories.Where(x => x.Text.Trim() == txtMove.Text.Trim()).FirstOrDefault();
            if (validateMove == null)
            {
                lblMsg.Text = "Move Item does not exists.";
                return;
            }
            if(validateMove.ParentId == 0)
            {
                lblMsg.Text = "Root Item can not be move.";
                return;
            }

            var validateTo = categories.Where(x => x.Text.Trim() == txtTo.Text.Trim()).FirstOrDefault();
            if (validateTo == null)
            {
                lblMsg.Text = "To Item does not exists";
                return;
            }

            treeView1.Nodes.Clear();
            Comment movedItem = categories.Where(x => x.Text == txtMove.Text.Trim()).FirstOrDefault();
            MovedItem = movedItem.Text;

            Comment item1 = categories.Where(x => x.Id == movedItem.ParentId).FirstOrDefault();
            MovedItemParent = item1.Text;
            
            List<Comment> lstChildren = categories.Where(x=> x.ParentId == movedItem.Id).ToList();
            if (lstChildren.Count > 0)
            {
                glstChildren = lstChildren;
                foreach (var child in lstChildren)
                {
                    child.ParentId= item1.Id;
                }
            }

            Comment newItem = categories.Where(x => x.Text == txtTo.Text.Trim()).FirstOrDefault();
            NewParent = newItem.Text;

            movedItem.ParentId = newItem.Id;

            CreateOrganization();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            treeView1.Nodes.Clear();
            Comment item = categories.Where(x => x.Text == MovedItem).FirstOrDefault();
            Comment item1 = categories.Where(x => x.Text == MovedItemParent).FirstOrDefault();

            item.ParentId = item1.Id;
            foreach(Comment child in glstChildren)
            {
                child.ParentId= item.Id;
            }
            CreateOrganization();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            treeView1.Nodes.Clear();
            Comment item = categories.Where(x => x.Text == MovedItem).FirstOrDefault();
            MovedItem = item.Text;
            Comment item1 = categories.Where(x => x.Id == item.ParentId).FirstOrDefault();
            MovedItemParent = item1.Text;
            Comment newItem = categories.Where(x => x.Text == NewParent).FirstOrDefault();
            NewParent = newItem.Text;

            item.ParentId = newItem.Id;
            foreach (Comment child in glstChildren)
            {
                child.ParentId = item1.Id;
            }
            CreateOrganization();

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            MovedItem = "";
            MovedItemParent = string.Empty;
            NewParent= string.Empty;
            glstChildren= new List<Comment>();
            treeView1.Nodes.Clear();
            
            SeedData();
            CreateOrganization();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            SeedData();
            CreateOrganization();
        }
    }
}
