def run(nums):
    result={}
    for i in nums:
        if i in result:
            del result[i]
        else:
            result[i]='goober'
    return result.pop()

print(run([25,14,7,1,22,25,22,7,1]))


