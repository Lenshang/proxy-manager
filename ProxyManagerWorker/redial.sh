echo 'redialing...'
sleep 1
ifdown ppp0
sleep 1
ifup ppp0
sleep 1
ps -ef|grep tinyproxy|grep -v grep|awk '{print "kill -9 "$2}'|sh
tinyproxy -c /etc/tinyproxy/tinyproxy.conf