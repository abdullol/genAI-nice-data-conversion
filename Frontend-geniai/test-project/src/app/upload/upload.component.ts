import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-upload',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './upload.component.html',
  styleUrl: './upload.component.scss',
  providers: [HttpClient],
})
export class UploadComponent {
  form: FormGroup;
  isDragging = false
  selectedFile: File | null = null
  apiKey = ""
  outputType = "userStories"
  additionalInstructions = ""
  generatedContent = ""

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.form = this.fb.group({
      file: [null],
      outputType: ['test-case'],
      instructions: ['']
    });
  }

  onDragOver(event: DragEvent) {
    event.preventDefault()
    event.stopPropagation()
    this.isDragging = true
  }

  onDragLeave() {
    this.isDragging = false
  }

  onDrop(event: DragEvent) {
    event.preventDefault()
    event.stopPropagation()
    this.isDragging = false

    if (event.dataTransfer?.files.length) {
      this.selectedFile = event.dataTransfer.files[0]
      console.log("File selected:", this.selectedFile.name)
    }
  }


  onFileSelected(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      this.selectedFile = fileInput.files[0]; // store the actual File object
    }
  }


  removeFile(): void {
    this.selectedFile = null;
  }

  generateContent() {
    if (!this.selectedFile) {
      console.error('No file selected!');
      return;
    }

    const outputType = this.form.value.outputType || 'text'; // default to 'text'
    if (!outputType) {
      console.error('outputType is required!');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);     // matches IFormFile file
    formData.append('outputType', outputType);      // matches string outputType in API

    this.http.post('https://localhost:7258/api/Upload/UploadFiles', formData).subscribe({
      next: (res) => console.log('Upload success:', res),
      error: (err) => console.error('Upload failed:', err)
    });

    this.generatedContent = "Processing...";
  }

}