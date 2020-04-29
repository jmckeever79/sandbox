def run(nums):
    result={}
    single='goober'
    for i in nums:
        if i in result:
            pass
        else:
            result[i]='goober'
            single=i
    if single=='goober':
        raise ValueError('single is goober')
    return single

print(run([25,14,7,1,22,25,22,7,1]))


