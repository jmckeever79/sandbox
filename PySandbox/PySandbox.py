import math

def run(nums):
    result={}
    for i in nums:
        if i in result:
            del result[i]
        else:
            result[i]='goober'
    return result.pop()

def happy_number(num):
    count = 0
    while True:
        num = sum_squares_of_digits(num)
        if (num == 1):
            return True
        count += 1
        if count > 100:
            return False

def sum_squares_of_digits(num):
    digits = extract_digits(num)
    s = sum([math.pow(x, 2) for x in digits])
    print('{0} --> {1}'.format(num, s))
    return s

def extract_digits(num):
    digits_ct = math.trunc(math.log(num, 10)) + 1
    result = []
    i = 0
    while i < digits_ct:
        val = num % (math.pow(10, i + 1))
        result.append(val/math.pow(10, i))
        num -= val
        i += 1
    return result 
        

#print(run([25,14,7,1,22,25,22,7,1]))
print(happy_number(19))


