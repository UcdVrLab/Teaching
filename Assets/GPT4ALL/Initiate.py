from gpt4all import GPT4All

#model = GPT4All("orca-mini-3b-gguf2-q4_0.gguf")
#output = model.generate("The capital of France is ", max_tokens=2)
#print(output)

model = GPT4All(model_name='orca-mini-3b-gguf2-q4_0.gguf')
with model.chat_session():
    print(model.generate(prompt='write me a short poem', temp=0))
    