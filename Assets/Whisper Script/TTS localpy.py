import pyttsx3

def my_python_function(input_string):
    engine = pyttsx3.init()
    engine.say(input_string)
    engine.runAndWait()
my_python_function("test")
