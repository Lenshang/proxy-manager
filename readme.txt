yum install tmux libevent libicu gcc gcc-c++ -y

firewall-cmd --zone=public --add-port=8787/tcp --permanent
firewall-cmd --reload

chmod 777 redial.sh



#安装tinyproxy
wget https://github.com/tinyproxy/tinyproxy/releases/download/1.11.0/tinyproxy-1.11.0.tar.gz
tar -zxvf tinyproxy-1.11.0.tar.gz
cd tinyproxy-1.11.0
./configure && make && make install

tinyproxy -v
find / -name tinyproxy.conf

vi /etc/tinyproxy/tinyproxy.conf
修改Port 8787
注释 #Allow 127.0.0.1

tinyproxy -c /etc/tinyproxy/tinyproxy.conf
ps -ef|grep tinyproxy|grep -v grep|awk '{print "kill -9 "$2}'|sh
