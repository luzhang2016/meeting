namespace Meeting.Admin.Models
{
    /// <summary>
    /// 部门结点类型
    /// </summary>
    public enum BranchNodeType
    {
        /// <summary>
        /// 模块
        /// </summary>
        Module,
        /// <summary>
        /// 树根
        /// </summary>
        Root,
        /// <summary>
        /// 树枝
        /// </summary>
        Branch,
        /// <summary>
        /// 叶子
        /// </summary>
        Leaf
    }

    /// <summary>
    /// 树结点
    /// </summary>
    public class ZtreeNodeModel
    {
        /// <summary>
        /// 模块Code
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get;
            set;
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public object IDKey
        {
            get;
            set;
        }

        /// <summary>
        /// 父节点唯一标识
        /// </summary>
        public object ParentIDKey
        {
            get;
            set;
        }

        /// <summary>
        /// 父结点ID
        /// </summary>
        public object ParentID
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 类型 BranchNodeType
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// 部门结点级别
        /// </summary>
        public BranchLevelType Level
        {
            get;
            set;
        }

        /// <summary>
        /// 性别
        /// </summary>
        public int Gender
        {
            get;
            set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public double Sort
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 部门结点级别类型
    /// </summary>
    public enum BranchLevelType
    {
        /// <summary>
        /// 无
        /// </summary>
        No,

        /// <summary>
        /// 一级
        /// </summary>
        One,

        /// <summary>
        /// 二级
        /// </summary>
        Tow,

        /// <summary>
        /// 三级
        /// </summary>
        Three
    }
}
