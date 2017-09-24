import Vue from 'vue'
import Router from 'vue-router'
import Index from '@/views/index'
import Meeting from '@/views/Meeting'
import Detail from '@/views/Detail.vue'
import Content from '@/views/content.vue'
import Member from '@/views/Member.vue'
import Record from '@/views/Record.vue'
import Register from '@/views/Register.vue'
import PersonalData from '@/views/PersonalData.vue'
import Statistic from '@/views/Statistic'
import Registered from '@/views/Registered'
import MeetingAgenda from '@/views/MeetingAgenda'
import downloadDetail from '@/views/downloadDetail'

Vue.use(Router)

export default new Router({
    // mode: 'history',
    // scrollBehavior: () => ({ y: 0 }),
    routes: [
        { path: '/', component: Index },
        { path: '/meeting/:uuid', component: Meeting },
        { path: '/detail/:uuid', component: Detail },
        { path: '/content/:uuid', component: Content },
        { path: '/member/:uuid', component: Member },
        { path: '/record/:uuid', component: Record },
        { path: '/register/:uuid', name: 'register', component: Register },
        { path: '/person', component: PersonalData },
        { path: '/statistic/:uuid', name: 'statistic', component: Statistic },
        { path: '/registered/:uuid', name: 'registered', component: Registered },
        { path: '/agenda/:uuid/:index', name: 'agenda', component: MeetingAgenda },
        { path: '/downloadDetail/:uuid/:index', name: 'downloadDetail', component: downloadDetail }
    ]
})