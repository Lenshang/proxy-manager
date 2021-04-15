import React from 'react';
import { Layout, Menu } from 'antd';
import { MenuUnfoldOutlined, MenuFoldOutlined } from '@ant-design/icons';
import { Route, Link, withRouter, RouteComponentProps, Redirect, Switch } from 'react-router-dom';
import MenuManager from './menu/MenuManager';
import ExLoading from '../components/ExLoading';
import api from '../api/Main';
import "./Main.css";

const { Header, Sider, Content } = Layout;
const { SubMenu } = Menu;
class Main extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            menu_collapsed: false,
            currentPage: undefined,
            logOut: false
        }
        this.menuManager = new MenuManager();
    }
    componentDidMount() {
        api.checkLogin();
    }

    componentWillMount() {
        //监听history,更新currentPage变量
        this.props.history.listen(location => {
            var obj = this.menuManager.getMenuByPath(location.pathname);
            if (obj) {
                this.setState({
                    currentPage: obj
                });
            }
            console.log(location.pathname);
        });

        //第一次访问时给currentPage变量赋值
        var obj = this.menuManager.getMenuByPath(this.props.location.pathname);
        if (obj) {
            this.setState({
                currentPage: obj
            });
        }
    }

    render() {
        /**
         * 触发菜单收缩
         * @param iscollapsed 是否收缩菜单
         */
        let menu_toggle = (iscollapsed) => {
            this.setState({
                menu_collapsed: iscollapsed,
            });
        };

        /**
         * 获得菜单
         * @param type 类型
         */
        let getMenuList = (type) => {
            if (type == "menu") {
                var r = this.menuManager.getMenuList().map(item => {
                    if (item.menuItems) {
                        return (
                            <SubMenu key={item.key} title={
                                <span>
                                    <item.icon></item.icon>
                                    {!this.state.menu_collapsed ? item.name : ""}
                                </span>
                            }>
                                {item.menuItems.map(m => {
                                    return (
                                        <Menu.Item key={m.key}>
                                            <m.icon></m.icon>
                                            <span>{m.name}</span>
                                            <Link to={m.path} />
                                        </Menu.Item>
                                    );
                                })}
                            </SubMenu>
                        );
                    }
                    else {
                        var _item = item;
                        return (
                            <Menu.Item key={_item.key}>
                                <item.icon></item.icon>
                                <span>{_item.name}</span>
                                <Link to={_item.path} />
                            </Menu.Item>
                        );
                    }
                })
                return r;
            }
            else {
                return (<Switch>
                    {this.menuManager.getAllMenu().map(item => {
                        return (<Route key={item.key} path={item.path} component={item.component} />);
                    })}
                    {/* <Route key="center" path="/" component={Center} /> */}
                    <Redirect exact from="/" to="/home"></Redirect>
                    <Route component={() => (
                        <Content>
                            <h1>页面没有找到</h1>
                        </Content>
                    )} />
                </Switch>)
            }
        }
        /**
         * 获得当前默认选中的菜单页面
         */
        let getDefaultSelectMenus = () => {
            let r = [];
            if (this.state.currentPage) {
                r.push(this.state.currentPage.key);
            }
            return r;
        }
        /**
         * 获得当前默认展开的页面菜单
         */
        let getDefaultOpenKeyMenu = () => {
            let r = [];
            if (this.state.currentPage) {
                let group = this.menuManager.getMenuGroupByMenu(this.state.currentPage);
                if (group) {
                    r.push(group.key);
                }
            }
            return r;
        }
        return (
            <Layout id="main-page">
                <Sider width="256px" breakpoint="md" className="main-sider" onBreakpoint={(broken) => { menu_toggle(broken); }} trigger={null} collapsible collapsed={this.state.menu_collapsed}>
                    {this.state.menu_collapsed ? (<div className="logo">P-M</div>) : (<div className="logo full-logo fade-in">Proxy-Manager</div>)}
                    <Menu theme="dark" mode="inline" selectedKeys={getDefaultSelectMenus()} defaultOpenKeys={getDefaultOpenKeyMenu()}>
                        {getMenuList("menu")}
                    </Menu>
                </Sider>
                <Layout className="site-layout">
                    <Header className="site-layout-background" style={{ padding: 0 }}>
                        <div style={{ display: "flex" }}>
                            {React.createElement(this.state.menu_collapsed ? MenuUnfoldOutlined : MenuFoldOutlined, {
                                className: 'trigger',
                                onClick: () => menu_toggle(!this.state.menu_collapsed),
                                style: { margin: 22, fontSize: 20 }
                            })}
                            <h1>{this.state.currentPage ? this.state.currentPage.fullName : "空"}</h1>
                            <div style={{ flex: 1, textAlign: "right", marginRight: "40px" }}><a onClick={() => {ExLoading.show(true);setTimeout(()=>{ExLoading.hide()},1000) }}>登出</a></div>
                        </div>
                    </Header>
                    <Content
                        style={{
                            margin: '24px 16px',
                            minHeight: 280,
                        }}>
                        {getMenuList("route")}
                    </Content>
                </Layout>
            </Layout>)
    }
}

export default withRouter(Main);