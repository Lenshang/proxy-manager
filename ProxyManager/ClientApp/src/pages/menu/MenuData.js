import React from 'react';
import { MenuObj, MenuGroup } from './MenuManager';
import { HomeOutlined, BlockOutlined,CloudServerOutlined,BuildOutlined,AppstoreOutlined } from '@ant-design/icons';
import NodeManager from '../NodeManager';
import Home from '../Home';
import ConfigPage from '../ConfigPage';
let menuData=[
    {
        key: "home",
        name: "首页",
        fullName: "首页",
        path: "/home",
        icon: HomeOutlined,
        component: Home
    },
    {
        key: "node-manager",
        name: "节点管理",
        fullName: "节点管理",
        path: "/node-manager",
        icon: CloudServerOutlined,
        component: NodeManager
    },
    {
        key: "settings",
        name: "设置",
        fullName: "设置",
        path: "/settings",
        icon: BuildOutlined,
        component: ConfigPage
    },
    {
        key: "about",
        name: "关于",
        fullName: "关于",
        path: "/about",
        icon: AppstoreOutlined,
        component: () => <h1>2021 Chen</h1>
    }
];

export default menuData;