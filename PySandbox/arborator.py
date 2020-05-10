class TreeNode(object):
    def __init__(self, val=0, left=None, right=None):
        self.val = val
        self.left = left
        self.right = right

class Arborator(object):
    def __init__(self):
        self.maxDiameter = -1

    def getMaxDepth(self, root):
        if root is None:
            return 0
        ld = self.getMaxDepth(root.left)
        rd = self.getMaxDepth(root.right)
        s = ld + rd
        if s > self.maxDiameter:
            self.maxDiameter = s
        return 1 + max(ld, rd)

    def diameterOfBinaryTree(self, root):
        if root.left is None and root.right is None:
            return 0

        ld = self.getMaxDepth(root.left)
        rd = self.getMaxDepth(root.right)
        s = ld + rd
        if s > self.maxDiameter:
            self.maxDiameter = s

        return self.maxDiameter