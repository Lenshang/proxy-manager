import React from 'react';
import { HomeOutlined, BlockOutlined } from '@ant-design/icons';
import { AntdIconProps } from '@ant-design/icons/lib/components/AntdIcon';
import menuData from './MenuData';
class MenuManager {
    menuList = menuData;
    otherPageBreadMap = {};
    /**
     * 获得Menu列表
     * Feature:根据用户组选择性返回Menu列表
     */
    getMenuList() {
        return this.menuList
    }
    /**
     * 根据otherPageBreadMap中的映射,返回对应面包屑
     * @param routeString Map映射路径
     */
    getBreadName(routeString) {
        return this.otherPageBreadMap[routeString];
    }
    /**
     * 获得所有Menu列表flatmap
     */
    getAllMenu() {
        let r = [];
        this.menuList.forEach(item => {
            if (!item.menuItems) {
                r.push(item);
            }
            else {
                (item).menuItems.forEach(m => {
                    r.push(m);
                });
            }
        });
        return r;
    }
    /**
     * 根据路径URL获得menu对象
     * @param path 路径string
     */
    getMenuByPath(path) {
        let r = null;
        this.getAllMenu().some(item => {
            if (path.indexOf(item.path) == 0) {
                r = item;
                return true;
            }
        });
        return r;
    }
    /**
     * 根据menu对象获得所在的menuGroup
     * @param menu menu对象
     */
    getMenuGroupByMenu(menu) {
        let r = null;
        if (menu) {
            this.getMenuList().some(item => {
                if (item.menuItems) {
                    if (item.menuItems.some(subItem => subItem.key == menu.key)) {
                        r = item;
                        return true;
                    }
                }
            });
        }
        return r
    }
    //#region 单例模式
    static instance = undefined;
    static getInstance() {
        if (!this.instance) {
            this.instance = new MenuManager();
        }
        return this.instance;
    }
    //#endregion
}

export default MenuManager;