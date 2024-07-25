from flask import Flask, request, jsonify
import whisper
import os
import os.path
import numpy as np
import librosa
from werkzeug.utils import secure_filename

app = Flask(__name__)
model = whisper.load_model("base.en")

@app.route('/transcribe', methods=['POST'])
def transcribe_audio():
    if 'file' not in request.files:
        return jsonify({"error": "No file part"}), 400
    
    file = request.files['file']
    if file.filename == '':
        return jsonify({"error": "No selected file"}), 400

    filename = secure_filename(file.filename)
    temp_path = os.path.join("C:/Users/cave/Documents/Valentin Mikey projet/FYP-master/Whisper", filename)
    file.save(temp_path)

    os.path.isfile(temp_path)
    audio,sample_rate=librosa.load("C:/Users/cave/Documents/Valentin Mikey projet/FYP-master/Whisper/audio.wav")

    if audio.ndim != 1:
        os.remove(temp_path)
        return jsonify({"error": "Audio must be a 1D tensor"}), 400

    prediction = model.transcribe(audio)

    os.remove(temp_path)
    print(prediction["text"])
    
    return jsonify({"text": prediction["text"]})

if __name__ == '__main__':
    app.run(debug=True)
