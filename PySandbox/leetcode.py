import math

class ListNode(object):
    def __init__(self, val=0, next=None):
        self.val = val
        self.next = next

    def __str__(self):
        return '{0}'.format(self.val)

def linkedlist_middlenode(head):
    if head.next is None:
        return head
    d = {}
    i = 1
    while head.next is not None:
        d[i] = head.val
        head = head.next
        i += 1
    return d[i//2 + 1]

def run_mod():
    l = ListNode(1)
    l.next = ListNode(2)
    l.next.next = ListNode(3)
    l.next.next.next = ListNode(4)
    l.next.next.next.next = ListNode(5)
    print(linkedlist_middlenode(l))
    return

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


class StringModule(object):
    def semantic_string(self, s):
        j = 0
        while s[j] == '#':
            j += 1
        s_equiv = []
        for i in range(j, len(s)):
            c = s[i]
            if c == '#':
                if s_equiv:
                    s_equiv.pop()
            else:
                s_equiv.append(c)
        return s_equiv

    def equiv_strings(self, s, t):
        s_equiv = self.semantic_string(s)
        t_equiv = self.semantic_string(t)
        return s_equiv == t_equiv