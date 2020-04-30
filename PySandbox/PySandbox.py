import math
import numpy as np

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

def compute_max_contig(nums):
    l = len(nums)
    if l == 1:
        return nums[0]

    s = sum(nums)
    result = nums

    for i in range(0, l+1):
        for j in range(i, l+1):
            print('<{0},{1}>'.format(i,j))
            temp = nums[i:j]
            if len(temp) == 0:
                continue
            tempsum = sum(temp)
            if tempsum > s:
                s = tempsum
                result = temp

    return s

def max_crossing_sum(v, startix, mid, endix):
    print('Computing crossing sum for {0}--{1} at {2}'.format(startix, endix, mid))

    sm = 0
    leftsum = -math.inf
    r0 = range(mid, startix, -1)
    print('Computing left sum for {0}'.format(r0))
    for i in r0:
        sm = sm + v[i]
        if sm > leftsum:
            leftsum = sm

    sm = 0
    rightsum = -math.inf
    r1 = range(mid+1, endix)
    print('Computing right sum for {0}'.format(r1))
    for i in r1:
        sm = sm + v[i]
        if sm > rightsum:
            rightsum = sm

    print('Left sum is {0} and right sum is {1}'.format(leftsum, rightsum))
    return max(leftsum+rightsum, leftsum, rightsum)


def max_subarray_sum(vector, startix, endix):
    print('Computing subarray sum for {0} to {1}'.format(startix, endix))

    if startix == endix:
        print('Reached 1-d vector at {0} = {1}'.format(startix, vector[startix]))
        return vector[startix]

    mid = (startix + endix) // 2

    lh = max_subarray_sum(vector, startix, mid)
    rh = max_subarray_sum(vector, mid+1, endix)
    cr = max_crossing_sum(vector, startix, mid, endix)

    print('Vector is {0}'.format(v))
    print('Left sum is {0}, right sum is {1}, crossing sum is {2}'.format(lh, rh, cr))

    return max([lh, rh, cr])

#print(run([25,14,7,1,22,25,22,7,1]))
#print(happy_number(19))

v = [-2, -1, 3, -2, -1, 4]
m = max_subarray_sum(v, 0, len(v))
print(m)

