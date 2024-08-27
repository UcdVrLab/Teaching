from flask import Flask, request, jsonify
from faster_whisper import WhisperModel, download_model
import os
import librosa
from werkzeug.utils import secure_filename

app = Flask(__name__)

# Define the model path
MODEL_PATH = "C:/Users/UCDVR/OneDrive/Documents/experiment/Whisper/model"
MODEL_TYPE = "base.en"

# Download the model if it doesn't exist
if not os.path.exists(MODEL_PATH):
    download_model(MODEL_TYPE, MODEL_PATH)

# Load the model from the local path
model = WhisperModel(MODEL_PATH, device="cpu", compute_type="int8")

@app.route('/transcribe', methods=['POST'])
def transcribe_audio():
    if 'file' not in request.files:
        return jsonify({"error": "No file part"}), 400
    
    file = request.files['file']
    if file.filename == '':
        return jsonify({"error": "No selected file"}), 400

    filename = secure_filename(file.filename)
    temp_path = os.path.join("C:/Users/UCDVR/OneDrive/Documents/experiment/Whisper", filename)
    file.save(temp_path)

    if not os.path.isfile(temp_path):
        return jsonify({"error": "File not saved correctly"}), 500

    audio, sample_rate = librosa.load(temp_path)

    if audio.ndim != 1:
        os.remove(temp_path)
        return jsonify({"error": "Audio must be a 1D tensor"}), 400

    segments, info = model.transcribe(temp_path)
    transcription = " ".join([segment.text for segment in segments])

    os.remove(temp_path)
    print(transcription)

    return jsonify({"text": transcription})

if __name__ == '__main__':
    app.run(debug=True)