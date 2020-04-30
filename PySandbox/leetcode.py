def movezeroes(nums):
    l = len(nums)
    i = 0
    zct = 0
    processed = 0
    while i < l:
        x = nums[i]
        if x == 0:
            zct += 1
            for j in range(i+1, l):
                nums[j-1]=nums[j]
        processed += 1
        if sum(nums[i:]) == 0:
            break;

        if nums[i] != 0:
            i += 1

        if processed == l:
            break

    while zct > 0:
        nums[l-zct] = 0
        zct -= 1

    return nums
