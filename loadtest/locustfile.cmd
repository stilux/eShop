locust -f locustfile.py --headless -u 100000 -r 15 --run-time 15m --host http://arch.homework --step-load --step-users 25 --step-time 10s