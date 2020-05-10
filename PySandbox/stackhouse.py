import numpy as np

class Stackhouse(object):
    """alternative implementation of a stack-like object"""
    
    def __init__(self):
        self.stack = []

    def push(self, x):
        
        if not self.stack:
            self.stack.append((x, x))
            return
        current_min = self.stack[-1][1]
        self.stack.append((x, min(x, current_min)))
        
    def pop(self):
        self.stack.pop()

    def top(self):
        return self.stack[-1][0]

    def getMin(self):
        return self.stack[-1][1]
