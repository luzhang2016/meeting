import Login from './views/Login.vue'
import NotFound from './views/404.vue'
import Home from './views/Home.vue'
import Main from './views/Main.vue'
import Table from './views/nav1/Table.vue'
import Form from './views/nav1/Form.vue'
import user from './views/nav1/user.vue'
import Page4 from './views/nav2/Page4.vue'
import Page5 from './views/nav2/Page5.vue'
import Page6 from './views/nav3/Page6.vue'
import echarts from './views/charts/echarts.vue'

import Log from './views/nav4/Log.vue'
import tubiao from './views/nav4/tubiao.vue'
import Meeting from './views/nav1/Meeting.vue'

let routes = [{
        path: '/login',
        component: Login,
        name: '',
        hidden: true
    },
    {
        path: '/404',
        component: NotFound,
        name: '',
        hidden: true
    },
    //{ path: '/main', component: Main },
    {
        path: '/',
        component: Home,
        name: '会议管理',
        iconCls: 'el-icon-message', //图标样式class
        children: [
            { path: '/main', component: Main, name: '主页', hidden: true },
            { path: '/table', component: Table, name: '会议列表' },
            { path: '/form', component: Form, name: '创建会议' },
            { path: '/Meeting', component: Meeting, name: '编辑会议' },
            { path: '/user', component: user, name: 'Dialog' },

        ]
    },
    {
        path: '/',
        component: Home,
        name: '常用设置',
        iconCls: 'fa fa-id-card-o',
        children: [
            { path: '/page4', component: Page4, name: '短信模版' },
            { path: '/page5', component: Page5, name: '通知分组' }
        ]
    },
    {
        path: '/',
        component: Home,
        name: '',
        iconCls: 'fa fa-address-card',
        leaf: true, //只有一个节点
        children: [
            { path: '/page6', component: Page6, name: '导航三' }
        ]
    },
    {
        path: '/',
        component: Home,
        name: '报表管理',
        iconCls: 'fa fa-bar-chart',
        children: [
            { path: '/echarts', component: echarts, name: 'echarts' }
        ]
    },
    {
        path: '/',
        component: Home,
        name: '系统日志',
        iconCls: 'el-icon-edit',
        children: [
            { path: '/log', component: Log, name: '操作日志' },
            { path: '/tubiao', component: tubiao, name: 'tubiao' }
        ]
    },
    {
        path: '*',
        hidden: true,
        redirect: { path: '/404' }
    }
];

export default routes;